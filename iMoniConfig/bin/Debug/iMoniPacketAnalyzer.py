'''
This file is used to obtain the information in iMoni Hex packets.
The user has to copy and paste the required iMoni packet to
retrive its informatoin.

DEVELOPER: Lanka Wijesinghe.
DATE: 2020.04.27

'''

# General variables here.
actCodeLenDict={0x01:0x04, #ac_readAnalogIN	
                0x02:0x03, #ac_readDigitalIN
                0x03:0x02, #ac_setDigitalOut
                0x04:0x03, #ac_clearDigitalOut
                0x05:0x06, #ac_setIOLimits
                0x06:0x06, #ac_readIOLimits
                0x09:0x02, #ac_enableAlarm			
	            0x0A:0x02, #ac_dissableAlarm	
	            0x0B:0x02, #ac_enableIO
	            0x0C:0x02, #ac_dissableIO			
	            0x0F:0x03, #ac_changeIOMode	
                0x10:0x02, #ac_analogIN_High
	            0x11:0x02, #ac_analogIN_Low
	            0x12:0x02, #ac_analogIN_OK
	            0x13:0x02, #ac_digitalAlarm_High
	            0x14:0x02, #ac_digitalAlarm_Low
	            0x15:0x02, #ac_digitalOK_Low
	            0x16:0x02, #ac_digitalOK_High
	            0x18:0x03, #ac_readDigitalOutStatus
	            0x19:0x06, #ac_read32bitdata
	            0x1A:0x06, #ac_writeSerial
	            0x1B:0x0A, #ac_read64bitdata
	            0x1C:0x02, #ac_ppFail
	            0x1D:0x02, #ac_ppOk
            }

actCodeDetailDict={ 0x01:"readAnalogIN"	,
                    0x02:"readDigitalIN",
                    0x03:"setDigitalOut",
                    0x04:"clearDigitalOut",
                    0x05:"setIOLimits",
                    0x06:"readIOLimits",
                    0x09:"enableAlarm",
                    0x0A:"dissableAlarm",	
	                0x0B:"enableIO",
	                0x0C:"dissableIO",			
	                0x0F:"ac_changeIOMode",	
                    0x10:"analogIN_High",
                    0x11:"analogIN_Low",
                    0x12:"analogIN_OK",
                    0x13:"digitalAlarm_High",
                    0x14:"digitalAlarm_Low",
                    0x15:"digitalOK_Low",
                    0x16:"digitalOK_High",
                    0x18:"readDigitalOutStatus",
                    0x19:"read32bitdata",
                    0x1A:"writeSerial",
                    0x1B:"read64bitdata",
                    0x1C:"peripheralFail",
                    0x1D:"peripheralOk"
                } 

#packet type dictionary.
packetTypeDict={0x00:"Null packet",			# to descard the packet responses after processing.
	            0x01:"addPeripheral",
	            0x02:"remPeripheral",
	            0x03:"modifyPPIO",
	            0x04:"updatePPconfig",		#update PP IO settings
                0x05:"readPPconfig",        #read PP IO config settings.
                0x06:"updateDevConfig",	    #update iMoni config file
                0x07:"readDevConfig",		#read iMoni config file
                0x08:"updateIOConfig",		#update iMoni IO config file
                0x09:"readIOConfig",		    #read iMoni IO config file
                0x0A:"modifyiMoniIO",		#change iMoni/pp IOs
                0x0B:"dataRequest",          #Request a snap data packet.
                0x0C:"errList",
                0x0D:"suspendPeripheral",
                0x0E:"releasePeripheral",
                0x0F:"suspendAll",
                0x10:"releaseAll",
                0x11:"peripheralStatus",
                0x12:"PeripheralAlarm",
                0x13:"PeripheralData",
                0x14:"PeripheralFailAlarm",
                0x15:"peripheralSerialData",
                0x16:"iMoniBusy",
                0x17:"iMoniError",
                0x18:"changeUpdateIngerval",
                0x19:"changeConnectionSettings",
                0x1A:"heartBeat",			        #live indicator is used to notify the server that a particular iMoni LITE device is in operating condition)
                0x1B:"serverCommand",	            #Server command (command from server to carry out some IO tasks like setting of thresholds and alarms).
                0x1C:"ack",
                0x1D:"storedData",
                0x1E:"Success",
                0x1F:"Error",
                0x20:"pt_OA",		                #POver the air programming of iMoni LITE (OAP)
                0x21:"HarmonicData"
            }

#peripheral type dictionary
ppTypeDict={0x00:"iMoni_LITE",
            0x01:"iMoni_LITE1",
            0x02:"iMoni_LITE2",
            0x03:"iMoni_LITE3",
	        0x11:"IOext1",
	        0x12:"IOext2",
	        0x13:"IOext3",
	        0x14:"IOext4",	
	        0x21:"AC_energyMate1",    #china default meter
	        0x22:"AC_energyMate2",
	        0x23:"AC_energyMate3",   
	        0x24:"AC_energyMate4",
	        0x25:"AC_energyMate5",    
	        0x26:"AC_energyMate6",
	        0x27:"AC_energyMate7",    
	        0x28:"AC_energyMate8",	
	        0x91:"AC_energyMate11",    #Schneider Power Logic PM5350
	        0x92:"AC_energyMate12",	
	        0xA1:"AC_energyMate21",    #Circutor CVM-NRG96
	        0xA2:"AC_energyMate22",	
	        0xB1:"AC_energyMate31",    #Schneider EasyLogic PM1200
	        0xB2:"AC_energyMate32",	
	        0xC1:"AC_energyMate41",    #Schneider EasyLogic PM2100
	        0xC2:"AC_energyMate42",	
	        0xD1:"AC_energyMate51",      #Schneider EasyLogic PM1130H
	        0xD2:"energyMate52",			
	        0x31:"DC_energyMate41",	
	        0x32:"DC_energyMate42",
	        0x81:"THsensor1",
	        0x82:"THsensor2",
	        0x83:"THsensor3",
	        0x84:"THsensor4",	
	        0x41:"UIMate1",
            0x42:"UIMate2",
	        0x51:"Modbus1",
            0x52:"Modbus2",
            0x53:"Modbus3",
            0x54:"Modbus4",
            0x55:"Modbus5",
            0x56:"Modbus6",
	        0x60:"WiFiMate",
	        0x70:"RadioMate",
	        0x80:"peripheralAll"
        }

#get user input from the user
print("............................................")
print("iMoni PACKET Analyzer v2.04")
print("Last Updated: 04/01/2021")
print("............................................")


import sys

print(sys.argv[1])


while(1):
    print(" ")
    print("Paste iMoni packet here")
    print(" ")      

    pkt=sys.argv[1]
    #pkt=input()
    pkt=pkt.replace("{","",-1)
    pkt=pkt.replace("}","",-1)
    #retrieve iMoni packet data

    #pkt="3836313639333033333637313933343230313930363138313435343531171A0000005B"

    #Get IMEI number.
    imei=""

    for i in range(1,31,2):
        imei+=(pkt[i])

    #Get date
    years=""
    months=""
    days=""
    hours=""
    minutes=""
    seconds=""
    fmver=""
    pktType=""
    segCount=""
    dataLen=""
    dataSegment=""
    segStart=68  # this is the start of the data segnemts. the index of hex packet.

    #3836313639333033333637313933343230313930363138313435343531171A0000005B
    #get year
    for i in range(31,38,2):
        years+=(pkt[i])

    #get month
    for i in range(39,42,2):
        months+=(pkt[i])

    #get day
    for i in range(43,46,2):
        days+=(pkt[i])

    #get Hours
    for i in range(47,50,2):
        hours+=(pkt[i])

    #get minutes
    for i in range(51,54,2):
        minutes+=(pkt[i])

    #get seconds
    for i in range(55,58,2):
        seconds+=(pkt[i])

    fmver=pkt[58]+pkt[59] #get firmware version
    pktType=pkt[60]+pkt[61] #get packet type
    if(pktType=="1C"):
        segCount="00"
        dataLen="00"
    else:
        segCount=pkt[62]+pkt[63] #get segnemt count
        dataLen=pkt[64]+pkt[65]+pkt[66]+pkt[67]
    pktDetail=packetTypeDict[int(pktType,16)]

    print("--------------------------------------------------------------------------------------------------")
    print("IMEI:"+imei)
    print("Time:"+years+"/"+months+"/"+days+" "+hours+":"+minutes+":"+seconds)
    print("FwVer:"+fmver)
    print("pktType:"+pktType+" ["+pktDetail+"]")
    print("segCount:"+segCount)
    print("dataLen:"+dataLen+" ("+(str(int(dataLen,16)))+")")
    #print("-------------------------------------------------------------------------------------------------")
    for i in range(0,int(segCount,16)):
        segLen=int((pkt[segStart+4]+pkt[segStart+5]),16) # retrieve segnent length.
        duCount=int((pkt[segStart+2]+pkt[segStart+3]),16) #retrieve data unit count
        segment=pkt[segStart:segStart+segLen*2]
        duStart=segStart+6
        segHeader=pkt[segStart:segStart+6] # segment header is 6 chars long.
        ppType=segHeader[0:2]
        ppDetail=ppTypeDict[int(ppType,16)]
        print("")           # to get the next segment printed below the previous one.
        print("--------------------------------------------------------------------------------------------------")
        print("segment: "+segHeader+" ["+ppDetail+"]")
        print("--------------------------------------------------------------------------------------------------")
        Line="                                                                                               "
        ID=""
        if pktType=="14":  #peripheral fail alarm packet
            ID="ppID"
        else :
            ID="IOID"   
        alignedLine=(Line[:9]+"DataUnit"+Line)[:200]
        alignedLine=(alignedLine[:34]+"Action"+Line)[:200]
        alignedLine=(alignedLine[:59]+ID+Line)[:200]
        alignedLine=alignedLine[:84]+"DataValue"        
        print(alignedLine)
        
        #a for loop is needed here to iterate data units and seperate them.
        for j in range(0,int(duCount)):           
            actCode=int((pkt[duStart]+pkt[duStart+1]),16)
            duLen=actCodeLenDict[actCode]
            actCodeDetail=actCodeDetailDict[actCode]
            du=pkt[duStart:duStart+duLen*2]
            ioID=du[2:4]
            ioID=int(ioID,16)                                        

            if len(du)==6:
                dataValue=str(int(du[4:6],16))
                dataValue2=""
            elif len(du)==8:
                dataValue=str(int(du[4:8],16))
                dataValue2=""
            elif len(du)==12:
                dataValue=str(int(du[4:8],16))
                dataValue2=str(int(du[8:],16))                   
            else:
                dataValue=""
                dataValue2=""
            if pktType=="14":              #peripheral fail alarm packet
                ioNumber=ioID & 0x0f
                dataValue=ppTypeDict[ioID]
            else:
                ioNumber=ioID & 0x1f  
           
            Line="                                                                                                  "
            alignedLine=""
            alignedLine=(Line[:10]+du+Line)[:200]
            alignedLine=(alignedLine[:35]+actCodeDetail+Line)[:200]
            alignedLine=(alignedLine[:60]+str(ioNumber)+Line)[:200]
            alignedLine=alignedLine[:85]+dataValue+" "+dataValue2
            print(alignedLine)
            duStart=duStart+duLen*2            
        segStart+=segLen*2

    break















