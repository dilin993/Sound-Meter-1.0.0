/*
 * File:   newmain.c
 * Author: Chathura Niroshan
 *
 * Created on March 7, 2017, 10:31 PM
 */

#include <stdio.h>
#include <stdlib.h>
#include <p18f2550.h>
#include <xc.h>
#define _XTAL_FREQ 48000000 //20 for the Delays
#pragma config PLLDIV = 4
#pragma config CPUDIV = OSC1_PLL2
#pragma config FOSC   = HSPLL_HS
#pragma config WDT    = OFF
#pragma config LVP    = OFF
#pragma config BOR    = OFF
#pragma config MCLRE  = ON
#pragma config PWRT   = ON
#pragma config PBADEN = OFF


int    datach[4];
int    val[4];
char   data;
int    counters[4];
int    gCounter;
long   sum[4];

void ADCInit()
{
   //We use default value for +/- Vref

   //VCFG0=0,VCFG1=0
   //That means +Vref = Vdd (5v) and -Vref=GEN

   //Port Configuration
   //We also use default value here too
   //All ANx channels are Analog

   /*
      ADCON2

      *ADC Result Right Justified.
      *Acquisition Time = 2TAD
      *Conversion Clock = 32 Tosc
    * full conversion of a sample takes 11 TAD + 2 TAD
   */

   ADCON2=0b10001010;
}

//Function to Read given ADC channel (0-13)
unsigned int ADCRead(unsigned char ch)
{
   if(ch>13) return 0;  //Invalid Channel

   ADCON0=0x00;

   ADCON0=(ch<<2);   //Select ADC Channel

   ADON=1;  //switch on the adc module

   GODONE=1;  //Start conversion

   while(GODONE); //wait for the conversion to finish

   ADON=0;  //switch off adc

   return ADRES;
}

char UART_Init(const long int baudrate)
{
  unsigned int x;
  x = (_XTAL_FREQ - baudrate*64)/(baudrate*64);   //SPBRG for Low Baud Rate
  if(baudrate>=115200)                                       //If High Baud Rage Required
  {
    x = (_XTAL_FREQ - baudrate*16)/(baudrate*16); //SPBRG for High Baud Rate
    BRGH = 1;                                     //Setting High Baud Rate
    //BRG16 = 1;                                      //Set 16 bit asynchronous mode
  }
  if(baudrate>=200000)                                       //If High Baud Rage Required
  {
    x = (_XTAL_FREQ - baudrate*4)/(baudrate*4); //SPBRG for High Baud Rate
    BRGH = 1;                                     //Setting High Baud Rate
    BRG16 = 1;                                      //Set 16 bit asynchronous mode
  }
  if(x<256)
  {
    SPBRG = x;                                    //Writing SPBRG Register
    SYNC = 0;                                     //Setting Asynchronous Mode, ie UART
    SPEN = 1;                                     //Enables Serial Port
    TRISC7 = 1;                                   //As Prescribed in Datasheet
    TRISC6 = 1;                                   //As Prescribed in Datasheet
    CREN = 1;                                     //Enables Continuous Reception
    TXEN = 1;                                     //Enables Transmission
    return 1;                                     //Returns 1 to indicate Successful Completion
  }
  return 0;                                       //Returns 0 to indicate UART initialization failed
}

void UART_Write(unsigned char data)
{
  while(!TRMT);
  TXREG = data;
}

char UART_TX_Empty()
{
  return TRMT;
}

void UART_Write_Text(unsigned char *text)
{
  int i;
  for(i=0;text[i]!='\0';i++)
    UART_Write(text[i]);
}

char UART_Data_Ready()
{
  return RCIF;
}

char UART_Read()
{
  while(!RCIF);
  return RCREG;
}

void UART_Read_Text(unsigned char *Output, unsigned int length)
{
  unsigned int i;
  for(i=0;i<length;i++)
  Output[i] = UART_Read();
}

void main(void) {
    TRISBbits.RB0 = 0;
    UART_Init(9600);
    ADCInit();
    while(1){
    LATBbits.LB0 = 1;
        counters[0] = 0;
        counters[1] = 0;
        counters[2] = 0;
        counters[3] = 0;
        gCounter = 0;
        sum[0] = 0;
        sum[1] = 0;
        sum[2] = 0;
        sum[3] = 0;
  
        while(gCounter<150)
        {
            val[0] = ADCRead(0);
            val[1] = ADCRead(1);
            val[2] = ADCRead(2);
            val[3] = ADCRead(3);

            for(int i=0;i<4;i++)
            {
                if(val[i]>10)
                {
                    counters[i]=counters[i]+1;
                    sum[i] = sum[i] + val[i];
                }
            }
            gCounter++;
            __delay_ms(10);
        }
    
        for(int i=0;i<4;i++)
        {
            if(counters[i]>20)
                datach[i] = sum[i]/counters[i];
            else
                datach[i] = 0;
        }
        
        // CH0L
    data = datach[0] & 31;
    UART_Write(data);
    // CH0H
    data = (datach[0]>>5)|128;
    UART_Write(data);

    // CH1L
    data = (datach[1] & 31)|32;
    UART_Write(data);
    // CH1H
    data = (datach[1]>>5)|160;
    UART_Write(data);

    // CH2L
    data = (datach[2] & 31)|64;
    UART_Write(data);
    // CH2H
    data = (datach[2]>>5)|193;
    UART_Write(data);

    // CH3L
    data = (datach[3] & 31)|96;
    UART_Write(data);
    // CH3H
    data = (datach[3]>>5)|224;
    UART_Write(data);
    
    LATBbits.LB0 = 0;
    __delay_ms(100);
    
    }
    return;
}
