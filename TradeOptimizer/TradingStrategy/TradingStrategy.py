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
     if math.isnan(x) or math.isnan(x):
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

        self.k, self.d = self.I(tal.STOCH, self.data.High, self.data.Low, self.data.Close, fastk_period=5, slowk_period=1, slowk_matype=0, slowd_period=3, slowd_matype=0)             
        self.sma200 = self.I(tal.SMA, self.data.Close, timeperiod=200)
        self.sma50 = self.I(tal.SMA, self.data.Close, timeperiod=50)
        self.dema = self.I(tal.DEMA, self.data.Close, timeperiod=30)
        self.roc = self.I(tal.ROC, self.data.Close, timeperiod=4)
        self.longRoc = self.I(tal.ROC, self.data.Close, timeperiod=150)
        self.macd, self.mSignal, self.mHisto = self.I(tal.MACD, self.data.Close, fastperiod=5, slowperiod=15, signalperiod=2) 
        self.bbHigh, self.bbMid, self.bbLow, = self.I(tal.BBANDS, self.data.Close, timeperiod=20,nbdevup=2, nbdevdn=2, matype=0)
        self.bop = self.I(tal.BOP, self.data.Open, self.data.High, self.data.Low, self.data.Close)

        self.macdFiltered = [value for value in self.macd if not math.isnan(value)]
        self.signalFiltered = [value for value in self.mSignal if not math.isnan(value)]
        self.histoFiltered = [value for value in self.mHisto if not math.isnan(value)]
        self.sma200Filtered = [value for value in self.sma200 if not math.isnan(value)]
        self.sma50Filtered = [value for value in self.sma50 if not math.isnan(value)]
        self.demaFiltered = [value for value in self.dema if not math.isnan(value)]
        self.rocFiltered = [value for value in self.roc if not math.isnan(value)]
        self.longRocFiltered = [value for value in self.longRoc if not math.isnan(value)]

        self.confirmation = 0
        self.confidence = 0
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
        self.minLongRoc, self.maxLongRoc = self.I(tal.MINMAX, self.longRoc, 150)
        
    def next(self):
        if not self.position:
            self.timeSincePosClose = self.timeSincePosClose + 1  
        else:
            self.timeSincePosOpen = self.timeSincePosOpen + 1

        self.oldConfidence = self.confidence
        self.confidence = 0

        self.positionSize = math.floor((self.startingEquity*0.8) / self.data.Close);

        if not self.position:
            if isCloseEnough(self.geneticValues['buySma200Index'], normalize(self.minSma200[-1], self.sma200[-1], self.maxSma200[-1])):
                self.confidence = self.confidence + self.geneticValues['buySma200Confidence']
            if isCloseEnough(self.geneticValues['buySma50Index'], normalize(self.minSma50[-1], self.sma50[-1], self.maxSma50[-1])):
                self.confidence = self.confidence + self.geneticValues['buySma50Confidence']
            if isCloseEnough(self.geneticValues['buyDemaIndex'], normalize(self.bbLow[-1], self.dema[-1], self.bbHigh[-1])):
                self.confidence = self.confidence + self.geneticValues['buyDemaConfidence']
            if isCloseEnough(self.geneticValues['buyRocIndex'], normalize(self.minRoc[-1], self.roc[-1], self.maxRoc[-1])):
                self.confidence = self.confidence + self.geneticValues['buyRocConfidence']
            if isCloseEnough(self.geneticValues['buyLongRocIndex'], normalize(self.minLongRoc[-1], self.longRoc[-1], self.maxLongRoc[-1])):
                self.confidence = self.confidence + self.geneticValues['buyLongRocConfidence']
            if isCloseEnough(self.geneticValues['buyBopIndex'], normalize(-1, self.bop[-1], 1)):
                self.confidence = self.confidence + self.geneticValues['buyBopConfidence']
            if isCloseEnough(self.geneticValues['buyKIndex'], normalize(0, self.k[-1], 100)):
                self.confidence = self.confidence + self.geneticValues['buyKConfidence']
            if isCloseEnough(self.geneticValues['buyDIndex'], normalize(0, self.d[-1], 100)):
                self.confidence = self.confidence + self.geneticValues['buyDConfidence']
            if blib.crossover(self.k, self.d):
                self.confidence += self.geneticValues['buyKDCrossoverConfidence']
                self.confidence = self.confidence + self.geneticValues['buyDConfidence']
            if isCloseEnough(self.geneticValues['buyMacdIndex'], normalize(self.minMacd[-1], self.macd[-1], self.maxMacd[-1])):
                self.confidence = self.confidence + self.geneticValues['buyMacdConfidence'] 
            if isCloseEnough(self.geneticValues['buyMSignalIndex'], normalize(self.minSignal[-1], self.mSignal[-1], self.maxSignal[-1])):
                self.confidence = self.confidence + self.geneticValues['buyMSignalConfidence']
            if isCloseEnough(self.geneticValues['buyMHistoIndex'], normalize(self.minHisto[-1], self.mHisto[-1], self.maxHisto[-1])):
                self.confidence = self.confidence + self.geneticValues['buyMHistoConfidence']
            if blib.crossover(self.macd, self.mSignal):
                self.confidence += self.geneticValues['buyMacdCrossoverConfidence']
            if isCloseEnough(self.geneticValues['buyBBMidIndex'], normalize(self.bbLow[-1], self.bbMid[-1], self.bbHigh[-1])):
                self.confidence = self.confidence + self.geneticValues['buyBBMidConfidence']

            if (self.confidence >= 1) \
            and self.positionSize > 0:
                self.buy(size=self.positionSize, sl=self.data.Close*0.95)
                self.timeSincePosOpen = 0

        if self.position:
            if isCloseEnough(self.geneticValues['sellSma200Index'], normalize(self.minSma200[-1], self.sma200[-1], self.maxSma200[-1])):
                self.confidence = self.confidence + self.geneticValues['sellSma200Confidence']
            if isCloseEnough(self.geneticValues['sellSma50Index'], normalize(self.minSma50[-1], self.sma50[-1], self.maxSma50[-1])):
                self.confidence = self.confidence + self.geneticValues['sellSma50Confidence']
            if isCloseEnough(self.geneticValues['sellDemaIndex'], normalize(self.bbLow[-1], self.dema[-1], self.bbHigh[-1])):
                self.confidence = self.confidence + self.geneticValues['sellDemaConfidence']
            if isCloseEnough(self.geneticValues['sellRocIndex'], normalize(self.minRoc[-1], self.roc[-1], self.maxRoc[-1])):
                self.confidence = self.confidence + self.geneticValues['sellRocConfidence']
            if isCloseEnough(self.geneticValues['sellLongRocIndex'], normalize(self.minLongRoc[-1], self.longRoc[-1], self.maxLongRoc[-1])):
                self.confidence = self.confidence + self.geneticValues['sellLongRocConfidence']
            if isCloseEnough(self.geneticValues['sellBopIndex'], normalize(-1, self.bop[-1], 1)):
                self.confidence = self.confidence + self.geneticValues['sellBopConfidence']
            if isCloseEnough(self.geneticValues['sellKIndex'], normalize(0, self.k[-1], 100)):
                self.confidence = self.confidence + self.geneticValues['sellKConfidence']
            if isCloseEnough(self.geneticValues['sellDIndex'], normalize(0, self.d[-1], 100)):
                self.confidence = self.confidence + self.geneticValues['sellDConfidence']
            if blib.crossover(self.d, self.k):
                self.confidence += self.geneticValues['sellKDCrossunderConfidence']
            if isCloseEnough(self.geneticValues['sellMacdIndex'], normalize(self.minMacd[-1], self.macd[-1], self.maxMacd[-1])):
                self.confidence = self.confidence + self.geneticValues['sellMacdConfidence'] 
            if isCloseEnough(self.geneticValues['sellMSignalIndex'], normalize(self.minSignal[-1], self.mSignal[-1], self.maxSignal[-1])):
                self.confidence = self.confidence + self.geneticValues['sellMSignalConfidence']
            if isCloseEnough(self.geneticValues['sellMHistoIndex'], normalize(self.minHisto[-1], self.mHisto[-1], self.maxHisto[-1])):
                self.confidence = self.confidence + self.geneticValues['sellMHistoConfidence']
            if blib.crossover(self.mSignal, self.macd):
                self.confidence += self.geneticValues['sellMacdCrossunderConfidence']
            if isCloseEnough(self.geneticValues['sellBBMidIndex'], normalize(self.bbLow[-1], self.bbMid[-1], self.bbHigh[-1])):
                self.confidence = self.confidence + self.geneticValues['sellBBMidConfidence']

            if self.confidence >= 1:
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