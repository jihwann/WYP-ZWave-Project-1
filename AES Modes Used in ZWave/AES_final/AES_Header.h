#pragma once
/*
============================================================================

FileName    : AES.h

Author      : JiHwan Lim

Version     : 1.0

Description : Header file for AES.cpp

============================================================================
*/
#ifndef __AES_H__
#define __AES_H__

#include <cstdio>
#include <cstring>
#include <cassert>
#include <cstring>
#include <iostream>

typedef unsigned char u8;

extern void print(u8* state, int len);

class AES
{
private:
	u8 Sbox[256];
	u8 InvSbox[256];
	u8 w[11][4][4];

	void KeyExpansion(u8* key, u8 w[][4][4]);
	u8 FFmul(u8 a, u8 b);

	void SubBytes(u8 state[][4]);
	void ShiftRows(u8 state[][4]);
	void MixColumns(u8 state[][4]);
	void AddRoundKey(u8 state[][4], u8 k[][4]);

	void InvSubBytes(u8 state[][4]);
	void InvShiftRows(u8 state[][4]);
	void InvMixColumns(u8 state[][4]);

public:
	AES(u8* key = NULL);
	virtual ~AES();
	void SetKey(u8 *key);
	u8* Cipher(u8* input, u8* output);
	u8* InvCipher(u8* input, u8* output);
	void* Cipher(void* input, void *output, int length = 0);
	void* InvCipher(void* input, void *output, int length);
};

enum AESMode_t { MODE_OFB = 1, MODE_CFB, MODE_CBC, MODE_ECB, MODE_CBC_MAC };
class AESModeOfOperation {
private:
	AES * m_aes;
	AESMode_t	  m_mode;
	u8 m_key[16];
	u8 m_iv[16];
	//	bool		  m_firstround;
public:
	AESModeOfOperation();
	~AESModeOfOperation();
	void set_mode(AESMode_t _mode);
	//AESMode_t get_mode();
	void set_key(u8 *key);
	void set_iv(u8 *iv);
	int  Encrypt(u8 *input, int length, u8 *output);
	int  Decrypt(u8 *input, int length, u8 *output);
};
#endif 
