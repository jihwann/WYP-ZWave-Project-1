#define _CRT_SECURE_NO_WARNINGS //fopen error delete

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

int main()
{
	long lSize;
	char *buffer;

	FILE *pFile = fopen("S2_DEV.txt", "r");

	// ������ ũ�⸦ ISize �� ����
	fseek(pFile, 0, SEEK_END);
	lSize = ftell(pFile);
	rewind(pFile);
	// ��ü ������ ������ ���� �� ���� ������ ũ��� �޸𸮸� �Ҵ�
	buffer = (unsigned char*) malloc(sizeof(char)*lSize);
	// �� ������ ������ ���ۿ� ����
	fread((unsigned char *)buffer, 1, lSize, pFile);

	FILE *w_fp=fopen("1.txt", "w");
	unsigned char value_data[2]="\0";

	for(int i=0; i < lSize-1; i++)
	{		
		while(buffer[i]==' ' && buffer[i+2]==' ')
		{
			buffer[i]=buffer[i]+0x10;
			break;		
		}
		strncpy(value_data, &buffer[i], 1);
		fwrite(&value_data, sizeof(char), 1, w_fp);
	}
	free(buffer);
	fclose(pFile);
	return 0;
}