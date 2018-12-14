#include <SPIMemory.h>
#include <SPIFlash.h>

SPIFlash flash; // 'csPin' is the Chip Select pin for the flash module.
// #define MB(x) uint32_t(x*MiB)

void setup() {
  Serial.begin(115200);
 
    if (!flash.begin()) {
        flash.error(VERBOSE);
    }
    Serial.print("Connected !!\n\n");  
    
    Serial.print(flash.getJEDECID(), HEX);
    Serial.print('\n');

  long long a = 0x000000;
  int cnt = 0;

  for(int b = 0; b < 2097151; b++){
  //for(int b = 0; b < 50000; b++){  
  //for(int b = 0; b < 20; b++){  
    cnt++;
    if(cnt == 24){
     Serial.print('\n');
     cnt = 0; 
    }
    Serial.print(flash.readByte(a), HEX);
    Serial.print(" ");
    a++;
    
   }

//uint8_t data_buffer[100];
//size_t bufferSize = 100;

//  Serial.print(flash.readByteArray(0x000000, data_buffer, bufferSize, false));

}

void loop() {

  
}



