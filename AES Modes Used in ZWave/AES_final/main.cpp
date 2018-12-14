/*
============================================================================

FileName    : main.cpp

Author      : JiHwan Lim

Version     : 1.0

Description : Test for each AES mode

============================================================================
*/
/*
* Test Vector for AES128-OFB mode
* 0x2b, 0x7e, 0x15, 0x16, 0x28, 0xae, 0xd2, 0xa6, 0xab, 0xf7, 0x15, 0x88, 0x09, 0xcf, 0x4f, 0x3c
¤¤ Encryption Key
* 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F
¤¤ Initialization vector
* 0x6b, 0xc1, 0xbe, 0xe2, 0x2e, 0x40, 0x9f, 0x96, 0xe9, 0x3d, 0x7e, 0x11, 0x73, 0x93, 0x17, 0x2a
¤¤ Test vector
* 0x3b, 0x3f, 0xd9, 0x2e, 0xb7, 0x2d, 0xad, 0x20, 0x33, 0x34, 0x49, 0xf8, 0xe8, 0x3c, 0xfb, 0x4a
¤¤ Cipher text
*/

/*
* Test Vector for AES128-CBC mode
* 0x2b, 0x7e, 0x15, 0x16, 0x28, 0xae, 0xd2, 0xa6, 0xab, 0xf7, 0x15, 0x88, 0x09, 0xcf, 0x4f, 0x3c
¤¤ Encryption Key
* 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F
¤¤ Invitialization vector
* 0x6b, 0xc1, 0xbe, 0xe2, 0x2e, 0x40, 0x9f, 0x96, 0xe9, 0x3d, 0x7e, 0x11, 0x73, 0x93, 0x17, 0x2a
¤¤ Test vector
* 0x76, 0x49, 0xab, 0xac, 0x81, 0x19, 0xb2, 0x46, 0xce, 0xe9, 0x8e, 0x9b, 0x12, 0xe9, 0x19, 0x7d
¤¤ Cipher text
*/

/*
* Test Vector for AES128-ECB mode
* 0x2b, 0x7e, 0x15, 0x16, 0x28, 0xae, 0xd2, 0xa6, 0xab, 0xf7, 0x15, 0x88, 0x09, 0xcf, 0x4f, 0x3c
¤¤ Encryption Key
* 0x6b, 0xc1, 0xbe, 0xe2, 0x2e, 0x40, 0x9f, 0x96, 0xe9, 0x3d, 0x7e, 0x11, 0x73, 0x93, 0x17, 0x2a
¤¤ Test vector
* 0x3a, 0xd7, 0x7b, 0xb4, 0x0d, 0x7a, 0x36, 0x60, 0xa8, 0x9e, 0xca, 0xf3, 0x24, 0x66, 0xef, 0x97
¤¤ Cipher text
*/
#define _CRT_SECURE_NO_WARNINGS
#include "AES_Header.h"
#include <vector>
using namespace std;

void print(u8* state, int len);
void AES_OFB_MODE(u8 *X, int size_X, u8 *key, u8 *IV, u8 *C);
void AES_CBC_MODE(u8 *key, u8 *IV, u8 SH, u8 SRC, u8 DST, int len_C, u8 *C, u8 *MAC);
void AES_ECB_MODE(u8 *input, int size_Passwd, u8 *key, u8 *output);
void AES_CBCMAC(u8 *km, u8 *header, int size_header, u8 *data, int size_data, u8 *MAC, int size_MAC);


void String_to_Hex(char *file_text, u8 *target, int len);
void Eliminate(char *str, char ch);

int main(int argc, char* argv[])
{
	char test[10][255];
	//test[0] : Kn,
	//test[1] : Passwd_c,
	//test[2] : Passwd_m,
	//test[3] : IV,
	//test[4] : X
	//test[5] : SH
	//test[6] : SRC
	//test[7] : DST
	u8 key[16] = { 0, };
	u8 Passwd_c[16] = { 0, };
	u8 Passwd_m[16] = { 0, };
	u8 IV[16] = { 0, };
	u8 X[4] = { 0, }; // PlainText
	u8 Kc[16] = { 0, };
	u8 Km[16] = { 0, };
	u8 C[4] = { 0, };
	u8 SH = 0;
	u8 SRC = 0;
	u8 DST = 0;
	u8 MAC[16] = { 0, };
	u8 header[20] = { 0, };

	char dst[4];
	char nonce[24];

	//scanf_s("%s", nonce, sizeof nonce);
	
	FILE *file = NULL;
	file = fopen("test.txt", "r");

	if (file != NULL) {
		int i = 0;
		char strTemp[255];
		char *pStr;

		while (!feof(file)) {
			pStr = fgets(strTemp, sizeof(strTemp), file);
			strcpy(test[i], pStr);
			i++;
		}  
	}
	else printf("There is no String to read");

	//printf("<Readout>\n");
	//printf("Kn : %s\n", test[0]);
	//printf("Passwd_c : %s\n", test[1]);
	//printf("Passwd_m : %s\n", test[2]);
	//printf("IV : %s\n", test[3]);
	//printf("PlainText : %s\n", test[4]);
	fclose(file);

	printf("\nInput Dst Node Id (ex. 02): ");
	fgets(dst, sizeof(dst), stdin);
	strcpy(&test[7][1], dst);

	printf("\nInput Nonce Report (ex. 3C 1F 2A CB DF AB 90 2F): ");
	fgets(nonce, sizeof(nonce), stdin);
	strcpy(&test[3][23], nonce);

	Eliminate(test[0], ' ');
	Eliminate(test[1], ' ');
	Eliminate(test[2], ' ');
	Eliminate(test[3], ' '); // IV
	Eliminate(test[4], ' '); // PlainText : X
	Eliminate(test[5], ' ');
	Eliminate(test[6], ' ');
	Eliminate(test[7], ' ');

	
	int len_test0 = strlen(test[0]) - 1;
	int len_test1 = strlen(test[1]) - 1;
	int len_test2 = strlen(test[2]) - 1;
	int len_test3 = strlen(test[3]) ;
	int len_test4 = strlen(test[4]) - 1;
	int len_test5 = strlen(test[5]) - 1;
	int len_test6 = strlen(test[6]) - 1;
	int len_test7 = strlen(test[7]);  

	String_to_Hex(test[0], key, len_test0);
	String_to_Hex(test[1], Passwd_c, len_test1);
	String_to_Hex(test[2], Passwd_m, len_test2);
	String_to_Hex(test[3], IV, len_test3);
	String_to_Hex(test[4], X, len_test4);
	String_to_Hex(test[5], &SH, len_test5);
	String_to_Hex(test[6], &SRC, len_test6);
	String_to_Hex(test[7], &DST, len_test7);

	
	printf("\n IV : ");
	for (int i = 0; i < sizeof(IV) / sizeof(u8); i++) {
		printf("%x ", IV[i]);
	}
	printf("\n");

	printf("Dst Node Id : %x \n", DST);
	

	/*
	//Complete Converting String to Hex array!
	for (int i = 0; i < sizeof(key) / sizeof(u8); i++) {
		printf("%x ", key[i]);
	}
	printf("\n");
	for (int i = 0; i < sizeof(Passwd_c) / sizeof(u8); i++) {
		printf("%x ", Passwd_c[i]);
	}
	printf("\n");
	for (int i = 0; i < sizeof(Passwd_m) / sizeof(u8); i++) {
		printf("%x ", Passwd_m[i]);
	}
	printf("\n");
	for (int i = 0; i < sizeof(IV) / sizeof(u8); i++) {
		printf("%x ", IV[i]);
	}
	printf("\n");
	for (int i = 0; i < sizeof(X) / sizeof(u8); i++) {
		printf("%x ", X[i]);
	}
	printf("\n");
	*/

	int size_Passwd_c = sizeof Passwd_c;
	int size_Passwd_m = sizeof Passwd_m;
	int size_IV = sizeof IV;
	int size_X = sizeof X;


	printf("\n\n---------------------------Kc---------------------\n\n");
	AES_ECB_MODE(Passwd_c, size_Passwd_c, key, Kc);
	printf("\n\n---------------------------Km---------------------\n\n");
	AES_ECB_MODE(Passwd_m, size_Passwd_m, key, Km);
	printf("\n\n--------------------------------------------------\n\n");
	AES_OFB_MODE(X, size_X, Kc, IV, C);
	printf("\n\n--------------------------------------------------\n\n");
	//AES_CBC_MODE(key, IV, SH, SRC, DST, sizeof C, C, MAC);
	//AES_CBCMAC(u8 *km, u8 *header, int size_header, u8 *data, int size_data, u8 *MAC)
	//AES_ECB_MODE(input16Byte, sizeof input16Byte, km, MAC);
	memcpy(header, IV, 16);
	header[16] = SH;
	header[17] = SRC;
	header[18] = DST;
	header[19] = u8(sizeof C);
	AES_CBCMAC(Km, header, sizeof header, C, sizeof C, MAC, sizeof MAC);

	printf("\n\n--------------------------------------------------");
	printf("\nKc : ");
	print(Kc, sizeof Kc);
	printf("Km : ");
	print(Km, sizeof Km);
	printf("C : ");
	print(C, sizeof C);
	printf("MAC : ");
	print(MAC, sizeof MAC);

	//printf("\n\n %d", sizeof C);
	printf("\n\n\n");
	print(IV, sizeof IV);

	printf("\n===========final result (you have to send this packet)==========\n");
	printf("98 81 ");
	for (int i = 0; i < 8; i++) {
		printf("%02x ", IV[i]);
	}
	for (int i = 0; i < (int)(sizeof C); i++) {
		printf("%02x ", C[i]);
	}
	printf("%02x ", IV[8]);
	for (int i = 0; i < 8; i++) {
		printf("%02x ", MAC[i]);
	}

	return 0;
}

void AES_OFB_MODE(u8 *X, int size_X, u8 *key, u8 *IV, u8 *C) {
	u8 enc[100] = { 0, };
	u8 temp[100] = { 0, };
	u8 dec[100] = { 0, };

	AESModeOfOperation OFB;
	OFB.set_key(key);
	OFB.set_mode(MODE_OFB);
	OFB.set_iv(IV);

	printf("\n\nPlainText (AES128-OFB mode)\n");
	printf("len = %d\n", size_X);
	printf("<Plain Text>");
	print(X, size_X);

	memcpy(temp, X, size_X); // error ³¯µí?
	int len = OFB.Encrypt(temp, size_X, enc);
	printf("\n\nEncrypt (AES128-OFB mode)\n");
	printf("len = %d\n", len);
	printf("<Encrypt Text>");
	print(enc, len);
	memcpy(C, enc, len);
	printf("\n\nDecrypt (AES128-OFB mode)\n");
	len = OFB.Decrypt(enc, len, dec);
	printf("len = %d\n", len);
	printf("<Decrypt Text>");
	print(dec, len);
}


void AES_CBC_MODE(u8 *key, u8 *IV, u8 SH, u8 SRC, u8 DST, int len_C, u8 *C, u8 *MAC) {
	//u8 input[] = { 0x6b, 0xc1, 0xbe, 0xe2, 0x2e, 0x40, 0x9f, 0x96, 0xe9, 0x3d, 0x7e, 0x11, 0x73, 0x93, 0x17, 0x2a };
	u8 input[32] = { 0, };

	input[0] = SH;
	input[1] = SRC;
	input[2] = DST;
	input[3] = (u8)len_C;
	for (int i = 0; i < len_C; i++) {
		input[i + 4] = C[i];
	}

	u8 enc[100] = { 0, };
	u8 temp[100] = { 0, };
	u8 dec[100] = { 0, };

	AESModeOfOperation CBC;
	CBC.set_key(key);
	CBC.set_mode(MODE_CBC);
	CBC.set_iv(IV);

	int clen = sizeof input;
	printf("\n\nPlainText (AES128-CBC mode)\n");
	printf("len = %d\n", clen);
	printf("<Plain Text>");
	print(input, clen);

	memcpy(temp, input, sizeof input);
	int len = CBC.Encrypt(temp, clen, enc);
	memcpy(MAC, enc, len);
	printf("\n\nEncrypt (AES128-CBC mode)\n");
	printf("len = %d\n", len);
	printf("<Encrypt Text>");
	print(enc, len);
	printf("\n\nDecrypt (AES128-CBC mode)\n");
	len = CBC.Decrypt(enc, len, dec);
	printf("len = %d\n", len);
	printf("<Decrypt Text>");
	print(dec, len);
}

void AES_ECB_MODE(u8 *input, int size_Passwd, u8 *key, u8 *output) {
	u8 enc[100] = { 0, };
	u8 temp[100] = { 0, };
	u8 dec[100] = { 0, };

	AESModeOfOperation ECB;
	ECB.set_key(key);
	ECB.set_mode(MODE_CBC);

	printf("\n\nPlainText (AES128-ECB mode)\n");
	//printf("len = %d\n", size_Passwd);
	printf("len = %d\n", sizeof input);
	printf("<Plain Text>");
	print(input, size_Passwd);

	memcpy(temp, input, size_Passwd);
	int len = ECB.Encrypt(temp, size_Passwd, enc);
	printf("\n\nEncrypt (AES128-ECB mode)\n");
	printf("len = %d\n", len);
	printf("<Encrypt Text>");
	print(enc, len);
	memcpy(output, enc, 16);
	printf("\n\nDecrypt (AES128-ECB mode)\n");
	len = ECB.Decrypt(enc, len, dec);
	printf("len = %d\n", len);
	printf("<Decrypt Text>");
	print(dec, len);
}

void AES_CBCMAC(u8 *km, u8 *header, int size_header, u8 *data, int size_data, u8 *MAC, int size_MAC) 
{
	u8 input16Byte[16] = { 0, };
	//u8 *input = new u8[size_header + size_data]; //--> X
	u8 input[24] = { 0, }; //--> O
	//u8 *input = new u8(size_header + size_data); //--> X
	//u8 *input = (u8*)malloc((size_header + size_data) * sizeof(u8)); // --> X
	//int len = size_header + size_data;
	//u8 *input = (u8*)calloc(len, sizeof(int));
	//u8 *input = new u8(len);
	//u8 *input = (u8 *)malloc(len * sizeof(u8));
	//memset(input, 0, len);
	
	for (int i = 0; i < size_header; i++) {
		input[i] = header[i];
	}

	for (int i = 0; i < size_data; i++) {
		input[20 + i] = data[i];
	}
	
	for (int i = 0; i < 16; i++) {
		if (i >= sizeof input)
			input16Byte[i] = 0;
		else
			input16Byte[i] = input[i];
	}
	AES_ECB_MODE(input16Byte, 16, km, MAC);
	memset(input16Byte, 0, sizeof input16Byte);
	
	int cipherIndex;
	int blockIndex = 0;

	for (cipherIndex = 16; cipherIndex < sizeof input; cipherIndex++) {
		input16Byte[blockIndex] = input[cipherIndex];
		// intput16Byte[16~23],  blockIndex = 7
 		blockIndex++;

		if (blockIndex == 16) {
			for (int i = 0; i < 16; i++) {
				MAC[i] = input16Byte[i] ^ MAC[i];
			}
			memset(input16Byte, 0, sizeof input16Byte);
			blockIndex = 0;

			AES_ECB_MODE(MAC, 16, km, MAC);
		}
	}
	u8 test[16] = { 0, };
	if (blockIndex != 0) {
		for (int i = 0; i < 16; i++) {
			MAC[i] = input16Byte[i] ^ MAC[i];
		}
		AES_ECB_MODE(MAC, 16, km, MAC);
	}
}


void print(u8* state, int len)
{
	int i;
	for (i = 0; i<len; i++)
	{
		if (i % 16 == 0) printf("\n");
		printf("%x ", (int)(state[i] & 0xFF));
	}
	printf("\n");
}

void String_to_Hex(char *test, u8 *target, int len) {
	char ch;
	u8 us_file_text[40] = { 0, };

	for (int i = 0; i < len; i++) {
		ch = test[i];
		if (ch >= '0' && ch <= '9') us_file_text[i] = ch - '0';
		else if (ch >= 'a' && ch <= 'f') us_file_text[i] = ch - 'a' + 0xa;
		else if (ch >= 'A' && ch <= 'F') us_file_text[i] = ch - 'A' + 0xa;
	}

	if ((len % 2) == 1) {
		for (int i = 0; i < len - 1; i += 2) {
			target[i / 2] = (us_file_text[i] << 4) ^ (us_file_text[i + 1]);
		}
		target[len / 2] = 0x0F & us_file_text[len - 1];
	}
	else {
		for (int i = 0; i < len; i += 2) {
			target[i / 2] = (us_file_text[i] << 4) ^ us_file_text[i + 1];
		}
	}
}

void Eliminate(char *str, char ch) {
	for (; *str != '\0'; str++)
	{
		if (*str == ch)
		{
			strcpy(str, str + 1);
			str--;
		}
	}
}
