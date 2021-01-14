#!/usr/bin/env python
# coding: utf-8

# In[29]:


import csv
import time
from math import sin, cos, sqrt, atan2, radians
import re
from Tkinter import *
import Tkinter,tkFileDialog
# Data files
Header_flights=['passengerid', 'flightid' ,'departurecode' ,'destinationcode','departuretime','totaltime']
Header_airport=['airportname', 'airportcode' ,'Latitude','Longitude']

class Flights(object) :

    def __init__(self, passengerid, flightid ,departurecode ,destinationcode,departuretime,totaltime):
        self.passengerid=passengerid
        self.flightid = flightid
        self.departurecode = departurecode
        self.destinationcode = destinationcode
        self.departuretime = departuretime
        self.totaltime = totaltime
    def depart_time(self,epochtime):
        return  time.strftime("%H:%M:%S", time.gmtime(float(epochtime)))
    def arrival_time(self,epochtime,mins):
        return  time.strftime("%H:%M:%S", time.gmtime(float(epochtime)+float(mins)*60))
    def fight_time(self,mins):
        return  time.strftime("%H:%M:%S", time.gmtime(float(mins)*60))


class Flights_Distance(Flights) :
    def __init__(self,passengerid, flightid ,departurecode ,destinationcode,departuretime,totaltime,departure_lat,departure_lon,destination_lat,destination_lon):
        super(Flights_Distance,self).__init__(passengerid, flightid ,departurecode ,destinationcode,departuretime,totaltime)
        self.departure_lat = departure_lat
        self.departure_lon = departure_lon
        self.destination_lat = destination_lat
        self.destination_lon = destination_lon
    def distance(self,departure_lat,departure_lon,destination_lat,destination_lon):
        #R=6373.0  #km
        R=3959.0 #mile
        lat1 = radians(float(departure_lat))
        lon1 = radians(float(departure_lon))
        lat2 = radians(float(destination_lat))
        lon2 = radians(float(destination_lon))
        dlon = lon2 - lon1
        dlat = lat2 - lat1
        a = sin(dlat / 2)**2 + cos(lat1) * cos(lat2) * sin(dlon / 2)**2
        c = 2 * atan2(sqrt(a), sqrt(1 - a))

        flight_distance =round(R * c,2)
        return flight_distance

#reader
def reader(file,Header)  :
    reader_output=[]        # list of dictionary
    with open(file,'r') as inputfile :
        file_info=csv.DictReader(inputfile,Header)
        for line in file_info :
            reader_output.append(dict(line.iteritems()))
    #print("\n".join('{}'.format(line) for line in reader_output))
    return  reader_output

# pre-processing
def hamming_distance(s1, s2):
    """Return the Hamming distance between equal-length sequences"""
    if len(s1) != len(s2):
        return 100   # using 100 to avoid the running error
    else :
        return sum(el1 != el2 for el1, el2 in zip(s1, s2))
def detect_error_flights(inputdata_flights) :
    clean_data=[]
    error_data=[]

    for line in inputdata_flights :
        # genenal detect
        match1=re.search('[A-Z]{3}[0-9]{4}[A-Z]{2}[0-9]',line["passengerid"])
        match2=re.search('[A-Z]{3}[0-9]{4}[A-Z]',line["flightid"])
        match3=re.search('[A-Z]{3}',line["departurecode"])
        match4=re.search('[A-Z]{3}',line["destinationcode"])
        match5=re.search('[0-9]{10}',line["departuretime"])
        match6=re.search('[0-9]{1,4}',line["totaltime"])

        if match1!=None and match2!=None and match3!=None and match4!=None and match5!=None and match6!=None :
           clean_data.append(line)
        elif match1==None :
            line['error_type']='passengerid_error'
            error_data.append(line)
        elif match2==None :
            line['error_type']='flightid_error'
            error_data.append(line)
        elif match3==None :
            line['error_type']='departurecode_error'
            error_data.append(line)
        elif match4==None :
            line['error_type']='destinationcode_error'
            error_data.append(line)
        elif match5==None :
            line['error_type']='departuretime_error'
            error_data.append(line)
        elif match6==None :
            line['error_type']='totaltime_error'
            error_data.append(line)

    #print(len(error_data))
    return clean_data,error_data
def detect_error_airport(inputdata) :
    clean_data1=[]
    error_data1=[]
    for line in inputdata :
        match1=re.search('[A-Z\/]{3,20}',line["airportname"])
        match2=re.search('[A-Z]{3}',line["airportcode"])
        match3=re.search('-?[0-9]{1,13}\.[0-9]{1,13}',line["Latitude"])
        match4=re.search('-?[0-9]{1,13}\.[0-9]{1,13}',line["Longitude"])


        if match1!=None and match2!=None and match3!=None and match4!=None :
           clean_data1.append(line)
        else :
           error_data1.append(line)
    #print("\n".join('{}'.format(line) for line in error_data1))
    return clean_data1
def correct_error(error_data,clean_data)  :
    corrected_output=[]
    cannot_correct=[]
    for line in error_data :
         k=0
         if line["error_type"]=='passengerid_error' :
             lists=[j["passengerid"] for j in clean_data]
             for value in list(lists) :
                 if hamming_distance(line["passengerid"] ,value)==1 :
                     line["passengerid"]=value
                     del line["error_type"]
                     corrected_output.append(line)
                     k=1
                     break
             if k==0 :  cannot_correct.append(line)

         elif  line["error_type"]=='flightid_error' :
             lists=[[i["flightid"],i['departurecode'],i['destinationcode']] for i in clean_data]
             for value in lists :
                 if hamming_distance(line["flightid"]+line['departurecode']+line['destinationcode'] ,value[0]+value[1]+value[2])==1 :
                     line["flightid"]=value[0]
                     del line["error_type"]
                     corrected_output.append(line)
                     k=1
                     break
             if k==0 :  cannot_correct.append(line)
         elif  line["error_type"]=='departurecode_error' :
             lists=[[l["flightid"],l['departurecode'],l['destinationcode']] for l in clean_data]
             for value in list(lists) :
                 if hamming_distance(line["flightid"]+line['departurecode']+line['destinationcode'] ,value[0]+value[1]+value[2])==1 :
                     line["departurecode"]=value[1]
                     del line["error_type"]
                     corrected_output.append(line)
                     k=1
                     break
             if k==0 :  cannot_correct.append(line)
         elif  line["error_type"]=='destinationcode_error' :
             lists=[[n["flightid"],n['departurecode'],n['destinationcode']] for n in clean_data]
             for value in list(lists) :
                 if hamming_distance(line["flightid"]+line['departurecode']+line['destinationcode'] ,value[0]+value[1]+value[2])==1 :
                     line["destinationcode"]=value[2]
                     del line["error_type"]
                     corrected_output.append(line)
                     k=1
                     break
             if k==0 :  cannot_correct.append(line)
         else :
             cannot_correct.append(line)

    return corrected_output,cannot_correct
def preprocess(input) :
    cleandata,errordata=detect_error_flights(input)
    corrected_data,cannotfixed=correct_error(errordata,cleandata)
    return cleandata+corrected_data


#mapper_Q1
def mapper_Q1(mapper_input,key_column) :
    mapper_output=[]
    if len(mapper_input)!=0 :
       for line in mapper_input :
           key=line[key_column]
           if "flightid" in line.keys() :
              key_value=line["flightid"]
           else :
              key_value=None
           mapper_output.append({key:key_value})
    else :
        mapper_output=[]
    #print("\n".join('{}'.format(line) for line in  mapper_output))
    return mapper_output

#mapper-Q2
def mapper_Q2(mapper_input) :
    mapper_output=[]
    if len(mapper_input)!=0 :
       for line in mapper_input :
           F_object=Flights(**line)   #class instance
           depart_time=F_object.depart_time(line['departuretime'])
           arrival_time=F_object.arrival_time(line['departuretime'],line['totaltime'])
           flight_time=F_object.fight_time(line['totaltime'])
           mapper_output.append({(F_object.flightid,F_object.departurecode,F_object.destinationcode,depart_time,arrival_time,flight_time):F_object.passengerid})
    else :
        mapper_output=[]
    #print("\n".join('{}'.format(line) for line in  mapper_output))
    return mapper_output

#mapper-Q4
def mapper_Q4(mapper_input1,mapper_input2) :
    mapper_output=[]
    if len(mapper_input1)!=0 :
        for line1 in mapper_input1 :
            line1_extend={}
            for line2 in mapper_input2:
                if line1['departurecode']==line2['airportcode'] :
                    line1.update({'departure_lat':line2['Latitude'],'departure_lon':line2['Longitude']})
                if line1['destinationcode']==line2['airportcode'] :
                    line1.update({'destination_lat':line2['Latitude'],'destination_lon':line2['Longitude']})
            line1_extend=line1
            if len(line1_extend)==10  : # to make sure have matched both longitude and latitude
               F_object=Flights_Distance(**line1_extend)   #class instance
               Distance=F_object.distance(F_object.departure_lat,F_object.departure_lon,F_object.destination_lat,F_object.destination_lon)
               mapper_output.append({F_object.passengerid:Distance})
    else :
        mapper_output=[]
    #print("\n".join('{}'.format(line) for line in mapper_output))
    return mapper_output


#shuffling
def shuffling(shuffling_input) :
    shuffling_output={}
    if len(shuffling_input)!=0 :
       for line in shuffling_input :
           if line.keys()[0] not in shuffling_output :
              shuffling_output[line.keys()[0]]=[line.values()[0]]
           else :
              shuffling_output[line.keys()[0]].append(line.values()[0])
    else :
       shuffling_output={}
    #print("\n".join('{}:{}'.format(key,value) for key,value in sorted(shuffling_output.items())))
    return shuffling_output


#reducer_Q1
def reducer_Q1(reducer_input) :
    reducer_output={}
    if len(reducer_input.keys())!=0 :
       for key, values in reducer_input.iteritems() :
           reducer_output[key]=len(set(filter(None,values)))
    else :
       reducer_output={}
    #print("\n".join('{}:{}'.format(key,value) for key,value in sorted(reducer_output.items(),key = lambda (k,v): v, reverse=True)))
    return reducer_output    # dictionary


#reducer_Q2
def reducer_Q2(reducer_input) :
    reducer_output={}
    if len(reducer_input.keys())!=0 :
       for key, values in reducer_input.iteritems() :
           reducer_output[key]=list(set(values))
    else :
       reducer_output={}

    # for key,values in sorted(reducer_output.items()) :
    #      print(list(key))
    #      print("\n".join(values))
    # print("\n".join('{}:{}'.format(key,value) for key,value in sorted(reducer_output.items())))
    return reducer_output    # dictionary

def reducer_Q3(reducer_input) :
    reducer_output={}
    if len(reducer_input.keys())!=0 :
       for key, values in reducer_input.iteritems() :
           key=(key[0],key[1],key[2])
           reducer_output[key]=len(values)
    else :
       reducer_output={}
    #print("\n".join('{}:{}'.format(key,value) for key,value in sorted(reducer_output.items())))
    return reducer_output

def reducer_Q4(reducer_input) :
    reducer_output={}
    if len(reducer_input.keys())!=0 :
       for key, values in reducer_input.iteritems() :
           reducer_output[key]=sum(values)
    else :
       reducer_output={}
    #print ("\n".join('{}:{}'.format(key,value) for key,value in sorted(reducer_output.items(),key = lambda (k,v): v, reverse=True)))
    return reducer_output    # dictionary

#exporter
def exporter(exporter_input,filename) :
    # print("\n".join('{}:{}'.format(key,value) for key,value in sorted(exporter_input.items(),key = lambda (k,v): v, reverse=True)))
    with open(filename,'w') as exporter_file:
        for key,values in sorted(exporter_input.items(),key = lambda v : (k,v) , reverse=True):
        # for key,values in sorted(exporter_input.items(),key = lambda (k,v): v):
             exporter_file.write(key+":"+str(values)+"\n")

def exporter_Q2(exporter_input,filename) :
    with open(filename,'w') as exporter_file:
         for key,values in sorted(exporter_input.items()) :
              print(list(key))
              #print("\n".join(values))
              exporter_file.write(' '.join(key))
              exporter_file.write('\n')
              exporter_file.write('\n'.join(values))
              exporter_file.write('\n')

def exporter_Q3(exporter_input,filename) :
    #print("\n".join('{}:{}'.format(key,value) for key,value in sorted(exporter_input.items())))
    with open(filename,'w') as exporter_file:
         for key,values in sorted(exporter_input.items()) :
             exporter_file.write(' '.join(key)+":"+str(values)+"\n")

def exporter_errors(exporter_input,filename) :
    with open(filename,'w') as exporter_file:
          for line in sorted(exporter_input) :
              exporter_file.write(" ".join([line['passengerid'],line['flightid'] ,line['departurecode'] ,line['destinationcode'],line['departuretime'],line['totaltime']]))
              exporter_file.write("\n")
##################################################################################
# Question 1
def run_Q1() :

    #step0- reading data
    file1=file1box.get()
    file2=file2box.get()
    file_Q1=file3box.get()+r"/Question1.txt"
    reader_output1=reader(file1,Header_flights)
    reader_output2=reader(file2,Header_airport)
    # step1- pre-processing data
    pre_output1=preprocess(reader_output1)
    pre_output2=detect_error_airport(reader_output2)

    # step2-mapper
    mapper_output1=mapper_Q1(pre_output1,'departurecode')
    mapper_output2=mapper_Q1(pre_output2,'airportcode')
    mapper_output=mapper_output1+mapper_output2

    # step3-#shuffling
    shuffling_output=shuffling(mapper_output)
    # step4-#reducer
    reducer_output=reducer_Q1(shuffling_output)
    textbox.insert(END,"\n".join('{}'.format(line) for line in sorted(reducer_output)))
    #step5-#exporter
    exporter(reducer_output,file_Q1)
    textbox.delete(0.0,END)
#    textbox.insert(END,"\n".join('{}:{}'.format(key,value) for key,value in sorted(reducer_output.items(),key = lambda v : (k,v) , reverse=True)
##    textbox.insert(END,"\n".join('{}:{}'.format(key,value) for key,value in sorted(reducer_output.items(),key = v : lambda (k,v), reverse=True)))
# #################################################################################
# Answer files

#Question 2
def run_Q2() :
    # step-0 reading data
    file1=file1box.get()
    file_Q2=file3box.get()+r"/Question2.txt"
    reader_output1=reader(file1,Header_flights)   # list of dictionary
    # step-1- pre-processing data
    pre_output1=preprocess(reader_output1)
    # step-2 mapping flight information by passengerid instead of 1
    mapper_output=mapper_Q2(pre_output1)
    # step-3 shuffling
    shuffling_output=shuffling(mapper_output)
    # step-4 reduce the same passengerid
    reducer_output=reducer_Q2(shuffling_output)
    #step5-#exporter
    exporter_Q2(reducer_output,file_Q2)
    textbox.delete(0.0,END)
    for key,values in sorted(reducer_output.items()) :
        textbox.insert(END,list(key))
        textbox.insert(END,"\n")
        textbox.insert(END,"\n".join(values))
        textbox.insert(END,"\n")
#################################################################################

#Question 3
def run_Q3() :
    # step-0 reading data
    file1=file1box.get()
    file_Q3=file3box.get()+r"/Question3.txt"
    reader_output1=reader(file1,Header_flights)   # list of dictionary
    # step-1- pre-processing data
    pre_output1=preprocess(reader_output1)
    # step-2 mapping flight information by passengerid instead of 1
    mapper_output=mapper_Q2(pre_output1)
    # step-3 shuffling
    shuffling_output=shuffling(mapper_output)
    # step-4 reduce the same passengerid
    reducer_output=reducer_Q3(shuffling_output)
    #step5-#exporter
    exporter_Q3(reducer_output,file_Q3)
    textbox.delete(0.0,END)
    textbox.insert(END,"\n".join('{}:{}'.format(key,value) for key,value in sorted(reducer_output.items())))
#################################################################################
#Question 4
def run_Q4() :
    # step0- reading data
    file1=file1box.get()
    file2=file2box.get()
    file_Q4=file3box.get()+r"/Question4.txt"
    reader_output1=reader(file1,Header_flights)
    reader_output2=reader(file2,Header_airport)
    # step1- pre-processing data
    pre_output1=preprocess(reader_output1)
    pre_output2=detect_error_airport(reader_output2)
    # step-2 mapping  passengerid by Distance value instead of 1
    mapper_output=mapper_Q4(pre_output1,pre_output2)
    # step-3 shuffling
    shuffling_output=shuffling(mapper_output)
    # step-4 reduce the same passengerid
    reducer_output=reducer_Q4(shuffling_output)
    # #step5-#exporter
    exporter(reducer_output,file_Q4)
    textbox.delete(0.0,END)
    textbox.insert(END,"\n".join('{}:{}'.format(key,value) for key,value in sorted(reducer_output.items(),key = lambda v:(k,v), reverse=True)))
# ###################################################################
# error/corrected print
def run_errors_detected() :
    file1=file1box.get()
    error_file=file3box.get()+"/errors.txt"
    reader_output=reader(file1,Header_flights)
    cleandata,errordata=detect_error_flights(reader_output)
    #print("\n".join('{}'.format([line['passengerid'],line['flightid'] ,line['departurecode'] ,line['destinationcode'],line['departuretime'],line['totaltime']]) for line in sorted(errordata)))
    exporter_errors(errordata,error_file)
    textbox.delete(0.0,END)
    textbox.insert(END,"The total number errors detected :"+str(len(errordata))+"\n")
    textbox.insert(END,"\n".join('{}'.format([line['passengerid'],line['flightid'] ,line['departurecode'] ,line['destinationcode'],line['departuretime'],line['totaltime']]) for line in sorted(errordata)))

def run_error_corrected() :
    file1=file1box.get()
    corrected_file=file3box.get()+"/errors_corrected.txt"
    reader_output=reader(file1,Header_flights)
    cleandata,errordata=detect_error_flights(reader_output)
    corrected_data,cannotfixed=correct_error(errordata,cleandata)
    exporter_errors(corrected_data,corrected_file)
    #print("\n".join('{}'.format([line['passengerid'],line['flightid'] ,line['departurecode'] ,line['destinationcode'],line['departuretime'],line['totaltime']]) for line in sorted(corrected_data)))
    textbox.delete(0.0,END)
    textbox.insert(END,"The total number errors corrected :"+str(len(corrected_data))+"\n")
    textbox.insert(END,"\n".join('{}'.format([line['passengerid'],line['flightid'] ,line['departurecode'] ,line['destinationcode'],line['departuretime'],line['totaltime']]) for line in sorted(corrected_data)))

def inputfile1() :
    file = tkFileDialog.askopenfile(parent=window,mode='rb',title='Choose the file of AComp_Passenger_data')
    file1box.delete(0, END)
    file1box.insert(0, file.name)
def inputfile2() :
    file = tkFileDialog.askopenfile(parent=window,mode='rb',title='Choose a file of Top30_airports_LatLong')

    file2box.delete(0, END)
    file2box.insert(0, file.name)
def outputfolder() :
    file = tkFileDialog.askdirectory(parent=window,title='Choose a folder')
    file3box.delete(0, END)
    file3box.insert(0, str(file))
#################interface configuration########################
window=Tk()
# define four labels title author year isbn
window.title("26801076Coursework")
window.geometry("600x600+200+200")


# input files
button0_1=Button(window, text="Browse file of AComp_Passenger_data",width=30,command=inputfile1)
button0_1.pack()
f1=StringVar(None)
file1box=Entry(window,textvariable=f1)
file1box.pack()

button0_2=Button(window, text="Browse file of Top30_airports_LatLong", width=30, command=inputfile2)
button0_2.pack()
f2=StringVar(None)
file2box=Entry(window,textvariable=f2)
file2box.pack()
# outputfile
button0_3=Button(window, text="Browse folder to save answers",width=30,command=outputfolder)
button0_3.pack()
f3=StringVar(None)
file3box=Entry(window,textvariable=f3)
file3box.pack()

# run answer button
labelText=StringVar()
labelText.set("Please press the button: ")
label1=Label(window,textvariable=labelText,height=2)
label1.pack()
button1=Button(window, text="Question_1", width=15,command=run_Q1).pack()
button2=Button(window, text="Question_2", width=15,command=run_Q2).pack()
button3=Button(window, text="Question_3", width=15,command=run_Q3).pack()
button4=Button(window, text="Question_4", width=15,command=run_Q4).pack()
button5=Button(window, text="Errors_Detected", width=15,command=run_errors_detected).pack()
button6=Button(window, text="Errors_Corrected", width=15,command=run_error_corrected).pack()
# text box
scrollbar = Scrollbar(window)
scrollbar.pack(side=RIGHT, fill=Y)
textbox=Text(window,width=80,height=22,wrap=NONE,background="LightYellow",yscrollcommand=scrollbar.set)
textbox.pack()
scrollbar.config(command=textbox.yview)

window.mainloop()


# In[ ]:




