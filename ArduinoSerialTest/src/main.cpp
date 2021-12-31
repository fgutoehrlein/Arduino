#include <Arduino.h>

//Globals
char incomingData;
String appendIncomingData;

void setup(){
  Serial.begin(9600);
}

void loop(){
  while(Serial.available()>0){
    incomingData = Serial.read();
    appendIncomingData += incomingData;
  }
  if(incomingData == '#'){
    Serial.print("I recieved: ");
    Serial.println(appendIncomingData);
    incomingData = 0;
    appendIncomingData = "";
    Serial.flush();
  }
}