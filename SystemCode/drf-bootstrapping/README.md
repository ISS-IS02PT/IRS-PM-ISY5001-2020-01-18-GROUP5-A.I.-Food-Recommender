## 1. Follow Step 1 and Step 2 in this [link](https://github.com/ISS-IS02PT/ISR-MR-2020-01-18-IS02PT-GROUP5-FoodRec/tree/master/SystemCode/pyke-bootstrapping) to have Python installed, and virtual environment setup

## 2. Install Django and DjangoRestFramework
```bash
# Make sure virtual env is activated
$ source venv/sandbox/bin/activate
(sandbox) $

# Install the packages using pip
(sandbox) $ pip install django djangorestframework

Collecting django
  Using cached Django-3.0.3-py3-none-any.whl (7.5 MB)
Collecting djangorestframework
  Using cached djangorestframework-3.11.0-py3-none-any.whl (911 kB)
Collecting sqlparse>=0.2.2
  Using cached sqlparse-0.3.1-py2.py3-none-any.whl (40 kB)
Collecting pytz
  Using cached pytz-2019.3-py2.py3-none-any.whl (509 kB)
Collecting asgiref~=3.2
  Using cached asgiref-3.2.3-py2.py3-none-any.whl (18 kB)
Installing collected packages: sqlparse, pytz, asgiref, django, djangorestframework
Successfully installed asgiref-3.2.3 django-3.0.3 djangorestframework-3.11.0 pytz-2019.3 sqlparse-0.3.1
```

## 3. Run the bootstrapping example
```bash
# The source code should have been downloaded by git if you have synced the remote and local repo (under 'develop' branch)
(sandbox) $ git checkout develop
(sandbox) $ cd ISR-MR-2020-01-18-IS02PT-GROUP5-FoodRec/SystemCode/drf-bootstrapping/foodrec_proj

# Run the development server
(sandbox) $ python manage.py runserver
Watching for file changes with StatReloader
Performing system checks...

System check identified no issues (0 silenced).
March 02, 2020 - 12:58:08
Django version 3.0.3, using settings 'foodrec_proj.settings'
Starting development server at http://127.0.0.1:8000/
Quit the server with CONTROL-C.
```
Leave the development server running.


## 4. Open another terminal, and install an additional package (httpie) for client testing
```bash
(sandbox) $ pip install httpie
Collecting httpie
  Using cached httpie-2.0.0-py2.py3-none-any.whl (64 kB)
Collecting Pygments>=2.5.2
  Using cached Pygments-2.5.2-py2.py3-none-any.whl (896 kB)
Collecting requests>=2.22.0
  Using cached requests-2.23.0-py2.py3-none-any.whl (58 kB)
Collecting idna<3,>=2.5
  Using cached idna-2.9-py2.py3-none-any.whl (58 kB)
Collecting chardet<4,>=3.0.2
  Using cached chardet-3.0.4-py2.py3-none-any.whl (133 kB)
Collecting certifi>=2017.4.17
  Using cached certifi-2019.11.28-py2.py3-none-any.whl (156 kB)
Collecting urllib3!=1.25.0,!=1.25.1,<1.26,>=1.21.1
  Using cached urllib3-1.25.8-py2.py3-none-any.whl (125 kB)
Installing collected packages: Pygments, idna, chardet, certifi, urllib3, requests, httpie
Successfully installed Pygments-2.5.2 certifi-2019.11.28 chardet-3.0.4 httpie-2.0.0 idna-2.9 requests-2.23.0 urllib3-1.25.8
```

## 4. Test our REST API endpoints
### 4.1. The root api (the endpoint of our development server), this will contain all the available endpoints for our API system. For now there is only 1 endpoint for Food
``` bash
(sandbox) $ http http://127.0.0.1:8000/ 

HTTP/1.1 200 OK
Allow: OPTIONS, GET
Content-Length: 40
Content-Type: application/json
Date: Mon, 02 Mar 2020 13:04:10 GMT
Server: WSGIServer/0.2 CPython/3.8.1
Vary: Accept, Cookie
X-Content-Type-Options: nosniff
X-Frame-Options: DENY

{
    "foods": "http://127.0.0.1:8000/foods/"
} 
```

### 4.2. Endpoint to list all foods. I have created a database with just 2 foods
```bash
(sandbox) $ http http://127.0.0.1:8000/foods/

HTTP/1.1 200 OK
Allow: GET, POST, HEAD, OPTIONS
Content-Length: 210
Content-Type: application/json
Date: Mon, 02 Mar 2020 13:12:25 GMT
Server: WSGIServer/0.2 CPython/3.8.1
Vary: Accept, Cookie
X-Content-Type-Options: nosniff
X-Frame-Options: DENY

[
    {
        "id": 1,
        "name": "6-inch Oven Roasted Chicken Breast Sandwich, Subway",
        "selfLink": "http://127.0.0.1:8000/foods/1/"
    },
    {
        "id": 2,
        "name": "Chicken, breast, fried, skinless",
        "selfLink": "http://127.0.0.1:8000/foods/2/"
    }
]

```

### 4.3. Endpoint to list specific food by accessing it selfLink. These are currently all the fields available for a food

```bash
$ http "http://127.0.0.1:8000/foods/1/"
HTTP/1.1 200 OK
Allow: GET, PUT, PATCH, DELETE, HEAD, OPTIONS
Content-Length: 248
Content-Type: application/json
Date: Mon, 02 Mar 2020 13:09:03 GMT
Server: WSGIServer/0.2 CPython/3.8.1
Vary: Accept, Cookie
X-Content-Type-Options: nosniff
X-Frame-Options: DENY

{
    "group": "FAST FOODS",
    "id": 1,
    "name": "6-inch Oven Roasted Chicken Breast Sandwich, Subway",
    "nutrient_protein_amount": 23.0,
    "nutrient_protein_unit": "g",
    "nutrient_sugar_amount": 5.98,
    "nutrient_sugar_unit": "g",
    "selfLink": "http://127.0.0.1:8000/foods/1/"
}
```

## 5. Stop the development server
Return to the original development server terminal, and press CONTROL-C to quit

```bash
...

Starting development server at http://127.0.0.1:8000/
Quit the server with CONTROL-C.
[02/Mar/2020 13:12:25] "GET /foods/ HTTP/1.1" 200 210
[02/Mar/2020 13:12:34] "GET /foods/1/ HTTP/1.1" 200 248

^C (sandbox) $
```

  
# Update 18-Mar
## 1. Added a new API endpoint /calculateNutrientNeeds
``` bash
(sandbox) $ http http://127.0.0.1:8000/
HTTP/1.1 200 OK
Allow: OPTIONS, GET
Content-Length: 113
Content-Type: application/json
Date: Wed, 18 Mar 2020 09:23:34 GMT
Server: WSGIServer/0.2 CPython/3.8.1
Vary: Accept, Cookie
X-Content-Type-Options: nosniff
X-Frame-Options: DENY

{
    "calculateNutrientNeeds": "http://127.0.0.1:8000/calculateNutrientNeeds/",
    "foods": "http://127.0.0.1:8000/foods/"
}
```

## 2. Input the required Profile parameters, and the api returns the results
### Note: The calculation is just normal function, no PyKE for now
``` bash
(sandbox) $ http http://127.0.0.1:8000/calculateNutrientNeeds/ age==23 height==182 weight==12 gender==male activity==very_active
HTTP/1.1 200 OK
Allow: GET
Content-Length: 64
Content-Type: application/json
Date: Wed, 18 Mar 2020 09:25:14 GMT
Server: WSGIServer/0.2 CPython/3.8.1
Vary: Accept, Cookie
X-Content-Type-Options: nosniff
X-Frame-Options: DENY

{
    "calories": 1693.09,
    "carbs": 169.31,
    "fat": 56.44,
    "protein": 126.98
}
```
