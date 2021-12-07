from backtesting import Backtest, Strategy
import backtesting.lib as blib
import talib as tal
import pandas_datareader.data as web
import pandas as pd
import datetime as dt
import yfinance as yfin
from datetime import datetime, timedelta
import sys
import win32pipe, win32file, pywintypes
import time
import math
import os
import mmap
import json
import numpy as np

pipeCreated = False
communicationID = sys.argv[1]

def pipeCreate():
    global pipeCreated

    print("Creating pipe...")
    handle = win32file.CreateFile(r'\\.\pipe\NETtoPython' + communicationID, win32file.GENERIC_READ | win32file.GENERIC_WRITE, 0, None, win32file.OPEN_EXISTING, 0, None)
    
    res = win32pipe.SetNamedPipeHandleState(handle, win32pipe.PIPE_READMODE_MESSAGE, None, None)
    if res == 0:
        print(f"SetNamedPipeHandleState return code: {res}")

    print("Pipe established.")
    pipeCreated = True
    return handle

def readPipe(pipeHandle):

    bytesToRead = 0

    while(bytesToRead == 0):
        buffer, bytesToRead, result = win32pipe.PeekNamedPipe(pipeHandle, 0)
        time.sleep(0.01)

    core, input = win32file.ReadFile(pipeHandle, 64*1024)
    inputString = input.decode('utf-8')

    return inputString

def writePipe(pipeHandle, content):
    win32file.WriteFile(pipeHandle, str.encode(content))
    win32file.FlushFileBuffers(pipeHandle)

def connectSharedMemory():
    global pipeCreated

    print("Capturing shared memory...")
    handle = mmap.mmap(-1, 10000, "SharedNETPython" + communicationID, mmap.ACCESS_WRITE | mmap.ACCESS_READ)

    print("Shared memory captured.")
    pipeCreated = True
    return handle

def readSharedMemory(sharedMemoryHandle, count):

    while(True):
        sharedMemoryHandle.seek(0, os.SEEK_END)
        if sharedMemoryHandle.tell():
            sharedMemoryHandle.seek(0)
            break
        else:
            time.sleep(0.2)

    input = sharedMemoryHandle.read(count)
    inputString = input.decode('utf-8')

    return inputString

def isCloseEnough(x, y):
     if math.isnan(x) or math.isnan(x): #or math.isnan(deviation):
        return 0

     if x <= y + 0.03 and x >= y - 0.03:
        return 1
     else:
        return 0

def normalize(min, val, max):
    test = (val - min)/(max - min)

    return (val - min)/(max - min)

class GeneticStrategy(Strategy):

    geneticValues = {}
    rocDiff = 0

    def init(self):

        self.startingEquity = 600
        self.buyConfidenceArray = np.array([0.0, 0.0, 0.0, 0.0])
        self.sellConfidenceArray = np.array([0.0, 0.0, 0.0, 0.0])

        self.k, self.d = self.I(tal.STOCH, self.data.High, self.data.Low, self.data.Close, fastk_period=5, slowk_period=1, slowk_matype=0, slowd_period=3, slowd_matype=0)             
        self.sma200 = self.I(tal.SMA, self.data.Close, timeperiod=200)
        self.sma50 = self.I(tal.SMA, self.data.Close, timeperiod=50)
        self.dema = self.I(tal.DEMA, self.data.Close, timeperiod=20)
        self.demaRoc = self.I(tal.ROC, self.dema, timeperiod=10)
        self.roc = self.I(tal.ROC, self.data.Close, timeperiod=4)
        #self.longRoc = self.I(tal.ROC, self.data.Close, timeperiod=150)
        self.macd, self.mSignal, self.mHisto = self.I(tal.MACD, self.data.Close, fastperiod=5, slowperiod=15, signalperiod=2) 
        self.bbHigh, self.bbMid, self.bbLow, = self.I(tal.BBANDS, self.data.Close, timeperiod=20,nbdevup=2, nbdevdn=2, matype=0)
        self.bop = self.I(tal.BOP, self.data.Open, self.data.High, self.data.Low, self.data.Close)
        self.obv = self.I(tal.OBV, self.data.Close, self.data.Volume.astype(np.double))
        self.obvRoc = self.I(tal.ROC, self.data.Close, timeperiod=20)
        self.adx = self.I(tal.ADX, self.data.High, self.data.Low, self.data.Close, timeperiod=14)
        self.aroonDown, self.aroonUp = self.I(tal.AROON, self.data.High, self.data.Low, timeperiod=14)

        self.oldConfidence = 0
        self.timeSincePosClose = 0
        self.timeSincePosOpen = 0

        self.minMacd, self.maxMacd = self.I(tal.MINMAX, self.macd, 5)
        self.minSignal, self.maxSignal = self.I(tal.MINMAX, self.mSignal, 15)
        self.minHisto, self.maxHisto = self.I(tal.MINMAX, self.mHisto, 2)
        self.minSma200, self.maxSma200 = self.I(tal.MINMAX, self.sma200, 200)
        self.minSma50, self.maxSma50 = self.I(tal.MINMAX, self.sma50, 50)
        self.minDema, self.maxDema = self.I(tal.MINMAX, self.dema, 30)
        self.minRoc, self.maxRoc = self.I(tal.MINMAX, self.roc, 4)
        #self.minLongRoc, self.maxLongRoc = self.I(tal.MINMAX, self.longRoc, 150)

        self.stochCrossState = False
        self.stochBuyState = False
        self.stochSellState = False
        self.smaTrendState = False
        self.demaTrendState = False
        self.macdTrendState = False
        self.macdCrossState = False
        self.bbState = 0
        self.bopState = 0
        self.obvState = False
        self.aroonTrendState = False
        self.trendStrengthState = 1
        
    def next(self):
        if not self.position:
            self.timeSincePosClose = self.timeSincePosClose + 1  
        else:
            self.timeSincePosOpen = self.timeSincePosOpen + 1        

        #State determination
            #Stochastic
        if self.k[-1] <= 20 and self.d[-1] <= 20:
            self.stochBuyState = True
        else:
            self.stochBuyState = False
        if self.k[-1] >= 80 and self.d[-1] >= 80:
            self.stochSellState = True
        else:
            self.stochSellState = False

        if blib.crossover(self.k, self.d):
            self.stochCrossState = True
        if blib.crossover(self.d, self.k):
            self.stochCrossState = False

            #SMA
        if blib.crossover(self.sma50, self.sma200):
            self.smaTrendState = True
        if blib.crossover(self.sma200, self.sma50):
            self.smaTrendState = False

            #DEMA
        if self.demaRoc[-1] > 0 and self.data.Close[-1] > self.dema[-1]:
            self.demaTrendState = True
        if self.demaRoc[-1] < 0 and self.data.Close[-1] < self.dema[-1]:
            self.demaTrendState = False

            #MACD
        if self.macd[-1] > 0:
            self.macdTrendState = True
        else:
            self.macdTrendState = False

        if blib.crossover(self.macd, self.mSignal):
            self.macdCrossState = True
        if blib.crossover(self.mSignal, self.macd):
            self.macdCrossState = False

            #BB
        if normalize(self.bbLow[-1], self.bbMid[-1], self.bbHigh[-1]) < 0.05:
            self.bbState = -1
        elif normalize(self.bbLow[-1], self.bbMid[-1], self.bbHigh[-1]) > 0.95:
            self.bbState = 1
        else:
            self.bbState = 0

            #BOP
        if self.bop[-1] > -0.05 and self.bop[-1] < 0.05:
            if blib.crossover(self.bop, 0):
                self.bopState = 1
            if blib.crossover(0, self.bop):
                self.bopState = -1
        else:
            self.bopState = 0

            #OBV
        if self.obvRoc[-1] > 0:
            obvState = True
        if self.obvRoc[-1] < 0:
            obvState = False   
            
            #AROON
        if blib.crossover(self.aroonUp, self.aroonDown):
            self.aroonTrendState = True
        if blib.crossover(self.aroonDown, self.aroonUp):
            self.aroonTrendState = False

            #ADX
        if self.adx[-1] > 30:
            self.trendStrengthState = 1
        elif self.adx[-1] < 15:
            self.trendStrengthState = -1
        else:
            self.trendStrengthState = 0

        self.currentBuyConfidence = 0
        self.currentSellConfidence = 0

        #Confidence determination
        if self.stochBuyState:
            self.currentBuyConfidence += self.geneticValues['stochBuyConfidence']
        if self.stochSellState:
            self.currentSellConfidence += self.geneticValues['stochSellConfidence']
        if self.stochCrossState:
            self.currentBuyConfidence += self.geneticValues['stochCrossoverBuyConfidence']
        else:
            self.currentSellConfidence += self.geneticValues['stochCrossunderSellConfidence']

        if self.smaTrendState:
            self.currentBuyConfidence += self.geneticValues['smaBuyConfidence']
        else:
            self.currentSellConfidence += self.geneticValues['smaSellConfidence']
        if self.demaTrendState:
            self.currentBuyConfidence += self.geneticValues['demaBuyConfidence']
        else:
            self.currentSellConfidence += self.geneticValues['demaSellConfidence']

        if self.macdTrendState:
            self.currentBuyConfidence += self.geneticValues['macdBuyConfidence']
        else:
            self.currentSellConfidence += self.geneticValues['macdSellConfidence']
        if self.macdCrossState:
            self.currentBuyConfidence += self.geneticValues['macdCrossoverBuyConfidence']
        else:
            self.currentSellConfidence += self.geneticValues['macdCrossunderSellConfidence']

        if self.bbState == -1:
            self.currentBuyConfidence += self.geneticValues['bbBuyConfidence']
        if self.bbState == 1:
            self.currentSellConfidence += self.geneticValues['bbSellConfidence']

        if self.bopState == 1:
            self.currentBuyConfidence += self.geneticValues['bopBuyConfidence']
        if self.bopState == -1:
            self.currentSellConfidence += self.geneticValues['bopSellConfidence']

        if self.obvState:
            self.currentBuyConfidence += self.geneticValues['obvBuyConfidence']
        else:
            self.currentSellConfidence += self.geneticValues['obvSellConfidence']

        if self.aroonTrendState:
            self.currentBuyConfidence += self.geneticValues['aroonBuyConfidence']
        else:
            self.currentSellConfidence += self.geneticValues['aroonSellConfidence']

        if self.trendStrengthState == 1:
            self.currentBuyConfidence += self.geneticValues['trendStrengthModifier']
            self.currentSellConfidence += self.geneticValues['trendStrengthModifier']

        np.append(self.buyConfidenceArray, self.currentBuyConfidence)
        np.append(self.sellConfidenceArray, self.currentSellConfidence)

        self.avgBuyConfidence = 0
        self.avgSellConfidence = 0

        for x in range(-1, -5, -1):
            self.avgBuyConfidence += self.buyConfidenceArray[x]
            self.avgSellConfidence += self.sellConfidenceArray[x]

        self.avgBuyConfidence /= 4
        self.avgSellConfidence /= 4

        self.allowablePositionSize = math.floor((self.startingEquity*0.8) / self.data.Close);

        if not self.position:

            if self.geneticValues['stopLossIndex'] >= 1:
                self.geneticValues['stopLossIndex'] = 0.99
            if self.geneticValues['stopLossIndex'] < 0:
                self.geneticValues['stopLossIndex'] = 0.01

            if self.avgBuyConfidence >= self.geneticValues['buyConfidence'] and self.allowablePositionSize > 0:
                self.buy(size=self.allowablePositionSize, sl=self.data.Close*self.geneticValues['stopLossIndex'])
                self.timeSincePosOpen = 0
            
      
        if self.position:
            
            if self.avgSellConfidence >= self.geneticValues['sellConfidence']:
                self.sell(size=self.position.size)
                self.timeSincePosClose = 0
       


def main():
    global pipeCreated

    print("Python backtester started.")

    stockData = {}
    stockData["ORCL"] = yfin.download('ORCL', period="ytd", interval="1h")
    stockData["AAPL"] = yfin.download('AAPL', period="ytd", interval="1h")
    stockData["MSFT"] = yfin.download('MSFT', period="ytd", interval="1h")
    #stockData["BB"] = yfin.download('BB', period="ytd", interval="1h")
    stockData["FB"] = yfin.download('FB', period="ytd", interval="1h")

    runFlag = 1
    
    pipeCreated = False

    while runFlag:           
        try:

            if not pipeCreated:              
                pipeHandle = pipeCreate()
                sharedMemoryHandle = connectSharedMemory()
            else:
           
                requestString = readPipe(pipeHandle)
                reqStringSplits = str.split(requestString)
                print(f"Received: {requestString}")               

                if reqStringSplits[0] == "receiveValues":
                  
                    indexesString = readSharedMemory(sharedMemoryHandle, int(reqStringSplits[1]))
                    
                    GeneticStrategy.geneticValues = json.loads(indexesString)

                    writePipe(pipeHandle, "done")

                if requestString == "runTest":
                    try:
                        startingCash = 100000

                        resultProfit = 0
                        resultPFactor = 0
                        resultSharpe = 0
                        resultDuration = 0
                        resultTradeCount = 0

                        for stockName, stockValues in stockData.items():
                            backTest = Backtest(stockValues, GeneticStrategy, cash=startingCash, commission=0.03)                
                            testStatistics = backTest.run()
                            print(testStatistics)

                            if not float(testStatistics._get_value("# Trades")) == 0:
                                duration = (testStatistics._get_value("Avg. Trade Duration")) / pd.Timedelta(hours=1)
                            else:
                                duration = 0

                            profitFac = testStatistics._get_value("Profit Factor")

                            if(math.isnan(profitFac)):
                                profitFac = 0

                            resultProfit += float(testStatistics._get_value("Equity Final [$]") - startingCash)
                            resultPFactor += profitFac
                            resultSharpe += float(testStatistics._get_value("Sharpe Ratio"))
                            resultDuration += duration
                            resultTradeCount += float(testStatistics._get_value("# Trades"))

                        stockCount = len(stockData)

                        resultPFactor /= stockCount
                        resultSharpe /= stockCount
                        resultDuration /= stockCount

                        result = {"Profit": resultProfit, "Profit Factor" : resultPFactor, "Sharpe" : resultSharpe, \
                            "Duration": resultDuration, "Trade Count" : resultTradeCount}

                        result = json.dumps(result)

                        writePipe(pipeHandle, result)
                    except:         
                        result = "error"
                        print(sys.exc_info()[0])
                        print(sys.exc_info()[1])
                        print(sys.exc_info()[2])

                        writePipe(pipeHandle, result)
                    

        except pywintypes.error as e:
            if e.args[0] == 2:
                pipeCreated = False
                print("Pipe not found...")
                time.sleep(1)
            elif e.args[0] == 109:
                pipeCreated = False
                print("Pipe broken.")
                return

if __name__ == "__main__":
    main()
