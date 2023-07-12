import time
import sys
import os
import optparse
import subprocess

subprocess.call([sys.executable, "-m", "pip", "install", "pip", "--upgrade"])


def installpackage(onepackage):
    subprocess.call([sys.executable, "-m", "pip", "install", onepackage])
	
try:
    import serial
except:
    installpackage('pyserial')
    import serial

# import serial

class UX3:
    
    BOX = '0'
    
    POWER_ON  = '1'
    POWER_OFF = '2'
    
    PORT_1 = '1'
    PORT_2 = '2'
    PORT_3 = '3'
    
    def __init__(self, com):
        try:
            self.com = serial.Serial(port='COM' + str(com), baudrate=300, stopbits=serial.STOPBITS_ONE, bytesize=serial.EIGHTBITS, timeout=1)
        except:
            raise

    def send_raw_command(self, cmd_str):
        print("command: {0:s}".format(cmd_str))
        self.com.write(cmd_str)
    
    def set_power(self, pwr):
        if pwr == self.POWER_ON:
            self.send_raw_command('*PON')
        elif pwr == self.POWER_OFF:
            self.send_raw_command('*POF')
        else:
            pass
    
    def set_port_power(self, port, pwr):
        cmd_str = '*' + self.BOX + port + pwr
        self.send_raw_command(cmd_str)
        
    def __del__(self):
        try:
            del(self.com)
        except:
            pass


if __name__ == '__main__':

    parser = optparse.OptionParser()

    parser.add_option('-c', dest='com', help='COM port number (example: 5)', default=0, type='int')
    parser.add_option('-r', dest='raw', help='Raw UX3 command', default=None, type='str')
    
    parser.add_option('-p', dest='port', help='Select UX3 port', default=0, type='int')
    
    parser.add_option('--on', dest='on', help='Turn on', default=False, action='store_true')
    parser.add_option('--off', dest='off', help='Turn off', default=False, action='store_true')
    
    (clOpt, args) = parser.parse_args()

    if (clOpt.com == 0):
        print("ERROR: zero com port input\r\n")
        sys.exit(1)

    try:
        ux = UX3(clOpt.com)
    except:
        print("ERROR: can not initialize ux3\r\n")
        sys.exit(2)

    port = None
    
    if clOpt.raw is not None:
        ux.send_raw_command(clOpt.raw)
        sys.exit(0)
    elif clOpt.port>0 and clOpt.port<4:
        port = str(clOpt.port)
    
    power = None
    
    if clOpt.on:
        power = ux.POWER_ON        
    elif clOpt.off:
        power = ux.POWER_OFF
    else:
        print("ERROR: undefined power mode")
        sys.exit(3)
        
    if port is not None:
        ux.set_port_power(port, power)
    else:
        ux.set_power(power)

    del (ux)

    sys.exit(0)