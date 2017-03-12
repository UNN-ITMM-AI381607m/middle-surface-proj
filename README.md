#Middle Surface 2d Project
## TEAM: UNN_ITMM_AI381607m

### Description
    There is project which is aimed at implementing a program for creating middle surface 2d  

### Git repository structure  
  There are 3 main folders:  
  doc/  
  release_doc/  
  src/  

  and additional folder:  
  script/

 1) The "doc" folder contains all documents, which are required.  
	"doc" folder contains "conceptualization" folder, which consists of specific folders for each document, for example: tests, mockups, etc.  
	Specific folder has follow structure:  
	template/  
	mockup/  
	project/    
		1.a) "template" contains template doc.  
		1.b) "mockup" contains mockup doc.  
		1.c) "project" contains project docs, named  project_v.%major_version%.%minor_version%. Also this folder contains project.cfg file, which has actual file version. 
		For creating new version of document, you can use create_new_version.py from the "scripts" folder. This script requires doc path(with old version or without version), and  cfg file additionally by option(--cfg_file=...),If you ran the script not from the folder that directly contains the config file or your config file has name different from project.cfg (default name)  
		Examples:  
		python .../create_new_version.py test.txt --cfg_file=C:/test/project_with_different_name.cfg  
		python .../create_new_version.py test_v.0.1.txt  
		
		NOTE: if your document is folder and you want archive doc, you can use option --zip:  
		python .../create_new_version.py my_folder --zip
		NOTE: script will work only if the python is installed  
	
 2) The "release_doc" folder contains required completed documents for customer.  
 3) The "src" folder constains source files, which are needed to build project.  
 4) The "scripts" folder contains scripts usefull for project development.   

