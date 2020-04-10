## 1. Make sure Docker is installed on your system
* If it is not yet installed, look out for instructions here:
    * Docker for Mac: [Link](https://docs.docker.com/docker-for-mac/)
    * Docker for Windows: [Link](https://docs.docker.com/docker-for-windows/)
        * Note that for Windows, the 'Docker for Windows' option will not work together with Virtualbox. So the best way is actually using the previous supported option [Docker Toolbox on Windows](https://docs.docker.com/toolbox/toolbox_install_windows/)
        * Alternatively, Docker is already installed as part of the ISS-VM

```bash
# Checking docker version
$ docker --version
Docker version 19.03.8, build afacb8b
```

## 2. Make sure Google Cloud SDK is installed on your system
* This is needed because the docker image is stored on Google Cloud Platform (GCP)
* To install Cloud SDK: [Link](https://cloud.google.com/sdk/install)
* Again the ISS-VM already have it installed

```bash
# Run this command from the terminal on your machine or ISS-VM
$ gcloud version
Google Cloud SDK 281.0.0
alpha 2019.05.17
beta 2019.05.17
bq 2.0.53
core 2020.02.14
datalab 20190610
gsutil 4.47
kubectl 2020.02.07
Updates are available for some Cloud SDK components.  To install them,
please run:
  $ gcloud components update

# The output above suggested that we should update some of the Cloud SDK components, but that is ok
```

## 3. Authenticate Cloud SDK with GCP
```bash
# Execute the command, then copy the generated link and open it from a browser. (Screenshots below)
# Login using your Google account, and then allow access by clicking on the 'Allow' button, then copy the verfication code form the resulting page back here:
$ gcloud auth login --no-launch-browser
Go to the following link in your browser:

    https://accounts.google.com/o/oauth2/auth?client_id=32555940559.apps.googleusercontent.com&redirect_uri=urn%3Aietf%3Awg%3Aoauth%3A2.0%3Aoob&scope=https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fuserinfo.email+https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fcloud-platform+https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fappengine.admin+https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fcompute+https%3A%2F%2Fwww.googleapis.com%2Fauth%2Faccounts.reauth&code_challenge=W_91qoiR9TyFXX0V6Y5YDJaN0gHBfIcnCET9UT3o0j0&code_challenge_method=S256&access_type=offline&response_type=code&prompt=select_account


Enter verification code: 


# After you enter the verification code above, you should be logged in successfully. You can confirm that by running:
$ $ gcloud auth list
                     Credentialed Accounts
ACTIVE  ACCOUNT
*       kenly@sparkcorners.com

To set the active account, run:
    $ gcloud config set account `ACCOUNT`
```

<img src="../static/Cloud SDK - Login.png"
     style="display: block;" />
     <br>
<img src="../static/Cloud SDK - Code.png"
     style="display: block;" />

## 4. Authenticate local Docker with Google Container Registry
```bash
$ gcloud auth print-access-token | docker login -u oauth2accesstoken --password-stdin https://asia.gcr.io

WARNING! Your password will be stored unencrypted in /root/.docker/config.json.
Configure a credential helper to remove this warning. See
https://docs.docker.com/engine/reference/commandline/login/#credentials-store

Login Succeeded
```

## 5. Pull the image from Google Container Registry (GCR) to local
```bash
# Execute the following command to pull the image from GCR, or to refresh the local image if it is updated from GCR
# You will see the progress of layers of images being downloaded
$ docker pull asia.gcr.io/my-spark-gke-test-272407/foodrec_proj:develop-0.1

develop-0.1: Pulling from my-spark-gke-test-272407/foodrec_proj
c499e6d256d6: Already exists 
62b0f1bf7919: Already exists 
3b19c64bdfee: Already exists 
196a2aed8498: Already exists 
6230be1200bd: Already exists 
aa0a7f77aba0: Downloading [=>                                                 ]  966.4kB/32.1MB
f03e65830b93: Download complete 
c35de4560928: Downloading [=======================>                           ]  2.412MB/5.215MB
52225fa1f818: Download complete 
1ed37971bbee: Waiting 
aa585f100cac: Waiting 
860f634d96b6: Waiting 
87150fc9b080: Waiting 
8045f6be0e13: Waiting 
eef3caddbea7: Waiting 
ad9c81d63241: Waiting 
98d2c382fdc5: Waiting 
```

## 6. Create the container from the downloaded image
```bash
# -d: To run the container in the background
# -p: Map the container port 8001 to an available port on local computer, in this case we choose port 8002
$ docker run -d -p 8002:8001 asia.gcr.io/my-spark-gke-test-272407/foodrec_proj:develop-0.1

7f1988c1b95dc15b3d68b44f6daf610571eb84450a4b711bf300f3b45f6c6713

# The resulting output long ID above is the full ID of the container
```
## 7. Confirm that we can access the backend as per normal, using httpie tool, or from the browser
```bash
(sandbox) $ http http://localhost:8002/ 

HTTP/1.1 200 OK
Allow: OPTIONS, GET
Connection: keep-alive
Content-Length: 304
Content-Type: application/json
Date: Fri, 10 Apr 2020 08:03:11 GMT
Server: nginx/1.14.2
Vary: Accept, Cookie
X-Content-Type-Options: nosniff
X-Frame-Options: DENY

{
    "calculate-nutrient-needs-from-profile": "http://localhost:8002/calculate-nutrient-needs-from-profile/",
    "food-recommendation-from-nutrient-needs": "http://localhost:8002/food-recommendation-from-nutrient-needs/",
    "food-recommendation-from-profile": "http://localhost:8002/food-recommendation-from-profile/"
}
```

## 8. To terminate the running container
```bash
# Find the ID of the running container
$ docker ps -a
CONTAINER ID        IMAGE                                                           COMMAND                  CREATED             STATUS                    PORTS                    NAMES
7f1988c1b95d        asia.gcr.io/my-spark-gke-test-272407/foodrec_proj:develop-0.1   "/usr/bin/supervisord"   34 seconds ago      Up 32 seconds             0.0.0.0:8002->8001/tcp   affectionate_lamarr

# Terminate the container with the corresponding ID
$ docker rm -vf 7f1988c1b95d

# To terminate all running containers (in case you want to clean up many at the same time)
$ docker rm -vf $(docker ps -aq)
```





