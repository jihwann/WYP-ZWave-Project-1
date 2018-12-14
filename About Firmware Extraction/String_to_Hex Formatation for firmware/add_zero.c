#define _CRT_SECURE_NO_WARNINGS //fopen error delete

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

int main()
{
	long lSize;
	char *buffer;

	FILE *pFile = fopen("S2_DEV.txt", "r");

	// 파일의 크기를 ISize 에 저장
	fseek(pFile, 0, SEEK_END);
	lSize = ftell(pFile);
	rewind(pFile);
	// 전체 파일의 내용을 받을 수 있을 정도의 크기로 메모리를 할당
	buffer = (unsigned char*) malloc(sizeof(char)*lSize);
	// 그 파일의 내용을 버퍼에 저장
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