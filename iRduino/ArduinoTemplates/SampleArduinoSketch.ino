/*
iRduino - Arduino Sketch
 Author: Mark Silverwood
 
 Using TM1638 library from Ricardo Batista <rjbatista at gmail dot com>
 
 This Arduino Sketch interfaces to the iRduino windows application.
 This Sketch was generated on: 2/13/2013
 This Sketch is for a double (2) Display Unit Configuration
 
 */

#include <TM1638.h>
#include <TM1640.h>

//////Setup Here
#define dataPin 4
#define clockPin 3
#define strobePin1 2
#define TM1640dataPin2 1
#define TM1640clockPin2 0
#define NumberUnits 2
#define NumberTM1638Units 1
//////Setup Finished

#define startByte1 255
#define startByte2 37
#define endByte 180
#define messageLength 33

TM1638 module1(dataPin,clockPin,strobePin1,false,0);
TM1640 module2(TM1640dataPin2,TM1640clockPin2,false,0);

//// Variable Declarations
word leds; 
byte segments[8], TM1640segments[16], redLeds, greenLeds, intensity, unit, sum, i, readCount, checkerPosition;
byte buttons[NumberTM1638Units], oldbuttons[NumberTM1638Units], lastButtonSend[NumberTM1638Units];
long lastCheck, debounceDelay = 100;
boolean sendButtons = false;
byte messageHolder[messageLength], message[messageLength-4], messagePosition = -1;

void setup() {
  Serial.begin(0);
  for(int u = 0; u < NumberTM1638Units; u++)
  {
    oldbuttons[u] = 0;
  }
  intensity = 0;
  lastCheck = millis();
}

void loop() {
  buttonsCheck();
  if (Serial.available() > 0){
    messagePosition++;
    if(messagePosition == messageLength) messagePosition = 0;
    messageHolder[messagePosition] = Serial.read();
    messageChecker();
  }
}

void messageChecker()
{
  checkerPosition = (messagePosition == 0) ? messageLength-1 : messagePosition - 1;
  if(messageHolder[checkerPosition] != endByte) return; //end byte
  checkerPosition = (messagePosition == messageLength -1) ? 0 : messagePosition + 1;
  if(messageHolder[checkerPosition]!= startByte1) return; //start byte1
  checkerPosition = (checkerPosition == messageLength - 1) ? 0 : checkerPosition+1;
  if(messageHolder[checkerPosition]!= startByte2) return; // start byte 2
  checkerPosition++;
  readCount = 0;
  sum = 0;
  while (readCount < messageLength-4)
  {
    if(checkerPosition == messageLength) checkerPosition = 0;
    message[readCount] = messageHolder[checkerPosition];
    sum += messageHolder[checkerPosition];
    readCount++;
    checkerPosition++;
  }
  if(sum == messageHolder[messagePosition]) messageRead(message);
}

void messageRead(byte _message[]){
  i = 0;
  intensity = _message[i++];
  for(int u = 1; u <= NumberUnits; u++)
  {
    unit = _message[i++];
    if(u==2)
    {
      for(int x = 0;x<16;x++){
        TM1640segments[x] = _message[i++];
      }
      updateDisplay(unit,word(0,0),TM1640segments,intensity);
    }
    else
    {
      greenLeds = _message[i++];
      redLeds = _message[i++];
      for(int x = 0;x<8;x++){
        segments[x] = _message[i++];
      }
      updateDisplay(unit,word(greenLeds,redLeds),segments,intensity);
    }
  }
}

void buttonsCheck(){
  if ((millis() - lastCheck)<debounceDelay) return;
  lastCheck = millis();
  buttons[0] = module1.getButtons();
  sendButtons = false;
  for(int u = 0; u < NumberTM1638Units; u++)
  {
    if (buttons[u] != lastButtonSend[u] && buttons[u] != oldbuttons[u]){
      sendButtons = true;
    }
    oldbuttons[u] = buttons[u];
  }
  if(sendButtons){
    Serial.write(startByte1);
    for(int u = 0; u < NumberTM1638Units; u++)
    {
      lastButtonSend[u] = buttons[u];
      Serial.write(lastButtonSend[u]);
    }
  }
}

void updateDisplay(byte unit, word _leds, byte _segments[], byte _intensity){
  switch(unit){
  case 1:
    module1.setupDisplay(true,_intensity);
    module1.setLEDs(_leds);
    module1.setDisplay(_segments);
    break;
  case 2:
    module2.setupDisplay(true,_intensity);
    module2.setDisplay(_segments,16);
    break;
  }
}

