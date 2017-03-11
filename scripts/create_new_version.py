import sys, ConfigParser, re, argparse, os.path 

def is_file_exists(file) :
    if not os.path.exists(file) :
        print('Error: file ' + file + ' is not exists!')
        return False
    return True

def create_new_version (target_file, cfg_file) :
    print('Creating new version....')

    if not is_file_exists(target_file) or not is_file_exists(cfg_file) :
        return

    config = ConfigParser.RawConfigParser()
    config.read(cfg_file)
 
    if not config.has_section('version') or not config.has_option('version', 'major') or not config.has_option('version', 'minor'):
        print('Error: Wrong configuration! \n The section should be: \n [version] \n major=%number% \n minor=%number% \n') 
        return

    major = config.getint('version', 'major')
    minor = config.getint('version', 'minor')

    print('The version of file ' + target_file + ' is: ' + str(major) + '.' + str(minor))
    print('\n Do you make major changes? Please type y/n or yes/no: ')
    sys.stdout.flush()
    inp = raw_input()
    condition = inp == 'y' or inp == 'yes' or inp == 'Y' or inp == 'YES'
    major += 1 if condition else 0
    minor += 0 if condition else 1

    version = '_v.' + str(major) + '.' + str(minor)

    config.set('version', 'major', major)
    config.set('version', 'minor', minor)

    with open(cfg_file, 'w') as fout:
        config.write(fout)
        fout.close()

    file, ext = os.path.splitext(target_file)
    tuple = re.subn(r'_v\.\d+.\d+$', version, file)

    new_target_file = tuple[0] + ext
    if tuple[1] == 0 :
       new_target_file = re.sub(r'\.[a-zA-Z]+$', file, '_v_' + version)
       new_target_file = file + version + ext

    print('\n Now file is: ' + new_target_file)
    target = open(new_target_file, 'w')
    fin = open(target_file, 'r')
    target.write(fin.read())
    fin.close()
    target.close()


parser = argparse.ArgumentParser()
parser.add_argument('target_file', type=str, action='store', default='')
parser.add_argument('--cfg_file', type=str, action='store', default='project.cfg')
args = parser.parse_args()
create_new_version(args.target_file, args.cfg_file)