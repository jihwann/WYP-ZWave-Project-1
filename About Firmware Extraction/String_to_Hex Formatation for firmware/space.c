#define _CRT_SECURE_NO_WARNINGS //fopen error delete

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

int main()
{
	long lSize;
	char * buf;

	FILE *fp = fopen("2.txt", "r");

	fseek (fp, 0, SEEK_END);
	lSize = ftell(fp);
	rewind(fp);	
	buf = (unsigned char*) malloc(sizeof(char)*lSize);

	fread(buf, sizeof(char), lSize, fp);
	
	FILE *w_fp=fopen("result.hex", "w");
	unsigned char value_data[2]="\0";
	
	for(int i = 0; i < lSize-1; i++)
	{
		while(buf[i] == ' ')
		{
			i++;
			break;
		}	
		strncpy(value_data, &buf[i], 1);
		fwrite(&value_data, sizeof(char), 1, w_fp);
	}

	free(buf); 		
	fclose(w_fp);

	return 0;
}