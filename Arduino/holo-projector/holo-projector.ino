#include <Wire.h>
#include <Adafruit_PWMServoDriver.h>
#include <SimpleTimer.h>

#define SERVOMIN  250 
#define SERVOMID  400
#define SERVOMAX  450 

#define LEDOFF 0
#define LEDON 3000

#define WHITEPIN 0
#define BLUEPIN 1

struct HoloProjector{
  int leftMotorPin;
  int rightMotorPin;
  int leftMotorBase;
  int rightMotorBase;
};

SimpleTimer projectorMoveTimer;

HoloProjector projectors[1] = {{3, 2, 400, 400}};

int projectorCount = 1;

// called this way, it uses the default address 0x40
Adafruit_PWMServoDriver pwm = Adafruit_PWMServoDriver();

void setup(){
  pwm.begin();
  
  pwm.setPWMFreq(60);  // Analog servos run at ~60 Hz updates

  setupLights();

  resetProjectorPositions();

  projectorMoveTimer.setInterval(2000, moveProjectors);
}

void loop() {
  projectorMoveTimer.run();
  runLights();
}

void moveProjectors(){
  for(int i = 0; i < projectorCount; i++){
    moveProjector(projectors[i]);  
  }
}

void moveProjector(HoloProjector projector){
  int left = random(SERVOMIN, SERVOMAX);
  int right = random(SERVOMIN, SERVOMAX);

  positionProjector(projector, left, right);
}

void resetProjectorPositions(){
  for(int i = 0; i < projectorCount; i++){
    resetProjectorPosition(projectors[i]);
  }
}

void resetProjectorPosition(HoloProjector projector){
  positionProjector(projector, projector.leftMotorBase, projector.rightMotorBase);
}

void positionProjector(HoloProjector projector, int leftMotorPosition, int rightMotorPosition){
  pwm.setPWM(projector.leftMotorPin, 0, leftMotorPosition);
  pwm.setPWM(projector.rightMotorPin, 0, rightMotorPosition);
}

void setupLights(){
  pwm.setPWM(WHITEPIN, 0, LEDON);
  pwm.setPWM(BLUEPIN, 0, LEDOFF);
}

void runLights(){
    pwm.setPWM(BLUEPIN, 0, LEDON);
    delay(100);
    pwm.setPWM(BLUEPIN, 0, LEDOFF);
    delay(100);
}
