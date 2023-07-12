
#Parameters
#LOG_PATH= r"..\tasklist.txt"
LOG_PATH1= r"tasklist.txt"
bad_words = ['taskkill /IM cmd.exe /F', 'SUCCESS:']
###############################################################################
#Run
with open(LOG_PATH1) as badfile, open('New_taskkill.txt', 'w') as cleanfile:
    for line in badfile:
        clean = True
        for word in bad_words:
            if word in line:
                clean = False
        if clean == True:
            cleanfile.write(line)

#######################################################
#import os
#print('basename:    ', os.path.basename(__file__))
#print('dirname:     ', os.path.dirname(__file__))
#ab=os.path.abspath(os.curdir)
#log2=ab+"\\"+LOG_PATH1