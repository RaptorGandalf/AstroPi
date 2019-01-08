String ACTIVATION_CODE = "7390011c";
String ID = "H";

int WHO_ARE_YOU = 0;
int BLINK_ONCE = 1;
int BLINK_TWICE = 2;
int BLINK_THRICE = 3;

int INACTIVE_LED_PIN = 12;
int ACTIVE_LED_PIN = 11;

bool Active = false;

void setup()
{
  Serial.begin(9600);

  pinMode(LED_BUILTIN, OUTPUT);
  pinMode(INACTIVE_LED_PIN, OUTPUT);
  pinMode(ACTIVE_LED_PIN, OUTPUT);

  digitalWrite(INACTIVE_LED_PIN, HIGH);
}

void loop()
{
  if(!Active)
  {
    activate();
    return;
  }
  
	processInput();
}

void activate()
{
  if(Serial.available())
  {
    String serialData = Serial.readString();

    Serial.println(serialData);

    //digitalWrite(INACTIVE_LED_PIN, LOW);
    //delay(500);
    //digitalWrite(INACTIVE_LED_PIN, HIGH);

    if(serialData == ACTIVATION_CODE)
    {
      Active = true;

      digitalWrite(INACTIVE_LED_PIN, LOW);
      digitalWrite(ACTIVE_LED_PIN, HIGH);
    }
  }
}

void processInput()
{
  if(Serial.available())
  {
    int incomingByte = Serial.read();

    if(incomingByte == WHO_ARE_YOU)
    {
      Serial.write("H");
    }
    if(incomingByte == BLINK_ONCE)
    {
      blink(1);
    }
    else if(incomingByte == BLINK_TWICE)
    {
      blink(2);
    }
    else if(incomingByte == BLINK_THRICE)
    {
      blink(3);
    }
    else
    {
      Serial.print("Invalid!");
    }
    
  }
}

void blink(int times)
{
  for(int i = 0; i < times; i++)
  {
    digitalWrite(LED_BUILTIN, HIGH);   
    delay(500);                       
    digitalWrite(LED_BUILTIN, LOW);    
    delay(500);         
  }
}
