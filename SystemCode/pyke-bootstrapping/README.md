## 1. Make sure Python3 is installed in your environment. If not, details to download and install can be found in [the official Python site](https://www.python.org/downloads/) for your OS

```bash
# Checking python version
$ python3 --version
Python 3.8.1
```

## 2. Using Python virtual environment so that any new Python packages to be installed are not messed up with the local environment

```bash
# Create a new folder to store the virtual env config (anywhere is fine)
$ mkdir venv

# Create a new virtual env in this location, with the name 'sandbox'
$ python3 -m venv venv/sandbox

# Activate the 'sandbox' virtual env (you will see the environment name before the prompt after execution) <-- Make sure this step is done everytime you want to activate this virtual env
$ source venv/sandbox/bin/activate   
(sandbox) $
# For Windows, execute activate.bat in scripts folder
>cd venv/sandbox/scripts
>activate

# Confirm the python version in this virtual env (should be the same as the original local Python version)
(sandbox) $ python --version
Python 3.8.1

#Upgrade pip (Python package manager) if needed
(sandbox) $ pip list
Package    Version
---------- -------
pip        19.2.3 
setuptools 41.2.0 
WARNING: You are using pip version 19.2.3, however version 20.0.2 is available.
You should consider upgrading via the 'pip install --upgrade pip' command.)

(sandbox) $ pip install --upgrade pip

# Deactivate the virtual environment (the environment name in bracket will disappear and return to normal)
(sandbox) $ deactivate
$ 
```

## 3. Download PyKE
Download the zip file from https://sourceforge.net/projects/pyke/, and store it in the same location of sandbox

## 4. Install PyKE
```bash
# Make sure virtual env is activated
$ source venv/sandbox/bin/activate
(sandbox) $

# Locate and unzip the file inside venv/sandbox/
(sandbox) $ ls -l venv/sandbox/ | grep pyke
-rw-r--r--@  1 kenly  primarygroup  1903051 25 Feb 18:15 pyke3-1.1.1.zip
(sandbox) $ unzip pyke3-1.1.1.zip

# Navigate into the unzipped folder, and install PyKE
(sandbox) $ cd pyke3-1.1.1
(sandbox) $ pip install .
(sandbox) $ pip list
Package    Version
---------- -------
pip        20.0.2 
pyke       1.1.1  
setuptools 41.2.0 
```

## 5. Run the hello-kitty example
```bash
# The source code should have been downloaded by git if you have synced the remote and local repo
(sandbox) $ cd ISR-MR-2020-01-18-IS02PT-GROUP5-FoodRec/SystemCode/pyke-bootstrapping/hello-kitty

# This example has 3 files, please checkout the content of each file
(sandbox) $ ls -l 
-rw-r--r--  1 kenly  primarygroup  1693  1 Mar 12:38 driver.py # This is the main program
-rw-r--r--  1 kenly  primarygroup    45  1 Mar 12:41 facts.kfb # This is the 'facts' file
-rw-r--r--  1 kenly  primarygroup   507  1 Mar 12:41 fc_rules.krb # This is the 'rules' file

# Execute the example program, the last 3 lines are program compilation, the last 4 lines correspond to the output of the main function in driver.py
(sandbox) $ python driver.py 
writing [compiled_krb]/facts.fbc
writing [compiled_krb]/fc_rules_fc.py
writing [compiled_krb]/compiled_pyke_files.py
HelloKitty is an animal
Sam is not an animal
Sam loves HelloKitty
Curiosity kills HelloKitty

# Deactivate the virtual environment when done
(sandbox) $ deactivate
$ 
```





