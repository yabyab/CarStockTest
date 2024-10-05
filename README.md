# CarStock Management to Go
This is a simple Web API for dealer to adjust their car stock level management.

# Concepts of avaliable functionalities
This Simple API is separate into two parts:
1. The Dealer
2. The dealers owned car stock
   
In the Dealer part, dealers are available to:
1. Login to their existing dealer
2. Register a new dealer
3. Get their login name and email
4. Update their login name and email

In the dealer's owned car stock part, dealers are available to:
1. List all owned car information
2. Search their owned car by the make, model and year
3. Add a new car as their new owned stock with the car's make, model, year, quantity and price
4. Update the car information of specific owned car with a new make, model and year
5. Update the car quantity (Stock level) of specific owned car
6. Update the car price (Stock level) of specific owned car
7. Remove a car which no longer have stock or dealer wants to remove from their stock list

# Available API
| HTTP Method    | URI                  | Description                                                                                          | Auth          |
|----------------|----------------------|------------------------------------------------------------------------------------------------------|---------------|
| POST           | /api/Dealer/Login    | Login to the dealer by given name and email                                                          | No            |
| POST           | /api/Dealer/Register | register to a new dealer by given name and email                                                     | No            |
| GET            | /api/Dealer          | Get the logged in dealer name and email                                                              | Bearer Token  |
| PUT            | /api/Dealer          | Update logged in dealer to the given name and email                                                  | Bearer Token  |
| GET            | /api/Stock           | List All the car stock of the logged in dealer                                                       | Bearer Token  |
| POST           | /api/Stock           | Add a new car stock into the logged in dealer list of car stock                                      | Bearer Token  |
| DELETE         | /api/Stock           | Remove a car stock from the logged in dealer owned car stock                                         | Bearer Token  |
| GET            | /api/Stock/search    | Search a list of car stock owned by the logged in dealer according to the given model, make and year | Bearer Token  |
| POST           | /api/Stock/AdjQty    | Update a specific car logged in dealer owned car stock level                                         | Bearer Token  |
| POST           | /api/Stock/AdjPrice  | Update a specific car logged in dealer owned car price                                               | Bearer Token  |
| POST           | /api/Stock/AdjInfo   | Update a specific car logged in dealer owned car model, make and year                                | Bearer Token  |

# API Input Usage
| HTTP Method    | URI                  | Input format     | Input Schemas                                                                                      | Input Sample                                  |
|----------------|----------------------|------------------|----------------------------------------------------------------------------------------------------|-----------------------------------------------|
| POST           | /api/Dealer/Login    | application/json | {<br>&emsp;**"dealername"**	string<br>&emsp;&emsp;nullable: false,<br>&emsp;&emsp;range: 50 chars max,<br>&emsp;**"dealeremail"** string<br>&emsp;&emsp;nullable: false,<br>&emsp;&emsp;range: 50 chars max,<br>&emsp;&emsp;validation: email format,<br>} | {<br>&emsp;"dealername" : "rose",<br>&emsp;"dealeremail" : "rose@testmail.com"<br>} |
| POST           | /api/Dealer/Register | application/json | {<br>&emsp;**"dealername"**	string<br>&emsp;&emsp;nullable: false,<br>&emsp;&emsp;range: 50 chars max,<br>&emsp;**"dealeremail"** string<br>&emsp;&emsp;nullable: false,<br>&emsp;&emsp;range: 50 chars max,<br>&emsp;&emsp;validation: email format,<br>} | {<br>&emsp;"dealername" : "rose",<br>&emsp;"dealeremail" : "rose@testmail.com"<br>} |
| GET            | /api/Dealer          | N/A              | -             | - |
| PUT            | /api/Dealer          | application/json | {<br>&emsp;**"dealername"**	string<br>&emsp;&emsp;nullable: false,<br>&emsp;&emsp;range: 50 chars max,<br>&emsp;**"dealeremail"** string<br>&emsp;&emsp;nullable: false,<br>&emsp;&emsp;range: 50 chars max,<br>&emsp;&emsp;validation: email format,<br>} | {<br>&emsp;"dealername" : "rose",<br>&emsp;"dealeremail" : "rose@testmail.com"<br>} |
| GET            | /api/Stock           | N/A              | N/A           |
| POST           | /api/Stock           | application/json | {<br>&emsp;**"stockid"** integer($int32)<br>&emsp;&emsp;nullable: false,<br>&emsp;&emsp;range: 0 <= stockid,<br>&emsp;**"dealerid"** integer($int32)<br>&emsp;&emsp;nullable: false,<br>&emsp;&emsp;range: 0 < dealerid,<br>&emsp;**"make"** string<br>&emsp;&emsp;nullable: false,<br>&emsp;&emsp;range: 50 chars max,<br>&emsp;**"model"** string<br>&emsp;&emsp;nullable: false,<br>&emsp;&emsp;range: 50 chars max,<br>&emsp;**"year"** integer($int32)<br>&emsp;&emsp;nullable: false,<br>&emsp;&emsp;range: 1900 <= year <= This Year,<br>&emsp;**"price"** number($double)<br>&emsp;&emsp;nullable: false,<br>&emsp;&emsp;range: 0 <= price <= 9999999.99,<br>&emsp;**"quantity"** integer($int32)<br>&emsp;&emsp;nullable: false,<br>&emsp;&emsp;range: 0 <= quantity <= 9999999<br>} | {<br>&emsp;"stockid": 0,<br>&emsp;"dealerid": 3,<br>&emsp;"make": "VW",<br>&emsp;"model": "tegruan",<br>&emsp;"year": 2013,<br>&emsp;"price": 3000,<br>&emsp;"quantity": 1<br>} |
| DELETE         | /api/Stock           | application/json | {<br>&emsp;**"stockid"** integer($int32)<br>&emsp;&emsp;nullable: false,<br>&emsp;&emsp;range: 0 <= stockid,<br>&emsp;**"dealerid"** integer($int32)<br>&emsp;&emsp;nullable: false,<br>&emsp;&emsp;range: 0 < dealerid,<br>&emsp;**"make"** string<br>&emsp;&emsp;nullable: false,<br>&emsp;&emsp;range: 50 chars max,<br>&emsp;**"model"** string<br>&emsp;&emsp;nullable: false,<br>&emsp;&emsp;range: 50 chars max,<br>&emsp;**"year"** integer($int32)<br>&emsp;&emsp;nullable: false,<br>&emsp;&emsp;range: 1900 <= year <= This Year,<br>&emsp;**"price"** number($double)<br>&emsp;&emsp;nullable: false,<br>&emsp;&emsp;range: 0 <= price <= 9999999.99,<br>&emsp;**"quantity"** integer($int32)<br>&emsp;&emsp;nullable: false,<br>&emsp;&emsp;range: 0 <= quantity <= 9999999<br>} | {<br>&emsp;"stockid": 0,<br>&emsp;"dealerid": 3,<br>&emsp;"make": "VW",<br>&emsp;"model": "tegruan",<br>&emsp;"year": 2013,<br>&emsp;"price": 3000,<br>&emsp;"quantity": 1<br>} |
| GET            | /api/Stock/search    | application/json | {<br>&emsp;**"make"**	string<br>&emsp;&emsp;nullable: true,<br>&emsp;&emsp;range: 50 chars max,<br>&emsp;**"model"**	string<br>&emsp;&emsp;nullable: true,<br>&emsp;&emsp;range: 50 chars max,<br>&emsp;**"year"**	integer($int32)<br>&emsp;&emsp;nullable: true,<br>&emsp;&emsp;range: 1900 <= year <= This Year<br>} |{<br>&emsp;"make": "t",<br>&emsp;"model": "s",<br>&emsp;"year": 2020<br>} |
| POST           | /api/Stock/AdjQty    | application/json | {<br>&emsp;**"stockid"** integer($int32)<br>&emsp;&emsp;nullable: false,<br>&emsp;&emsp;range: 0 < stockid,<br>&emsp;**"quantity"** integer($int32)<br>&emsp;&emsp;nullable: false,<br>&emsp;&emsp;range: 0 <= quantity <= 9999999<br>} |{<br>&emsp;"stockid": 16,<br>&emsp;"quantity": 3<br>} |
| POST           | /api/Stock/AdjPrice  | application/json | {<br>&emsp;**"stockid"** integer($int32)<br>&emsp;&emsp;nullable: false,<br>&emsp;&emsp;range: 0 < stockid,<br>&emsp;**"price"** number($double)<br>&emsp;&emsp;nullable: false,<br>&emsp;&emsp;range: 0 <= price <= 9999999.99,<br>} |{<br>&emsp;"stockid": 16,<br>&emsp;"price": 3<br>} |
| POST           | /api/Stock/AdjInfo   | application/json | {<br>&emsp;**"stockid"** integer($int32)<br>&emsp;&emsp;nullable: false,<br>&emsp;&emsp;range: 0 < stockid,<br>&emsp;**"make"** string<br>&emsp;&emsp;nullable: false,<br>&emsp;&emsp;range: 50 chars max,<br>&emsp;**"model"** string<br>&emsp;&emsp;nullable: false,<br>&emsp;&emsp;range: 50 chars max,<br>&emsp;**"year"** integer($int32)<br>&emsp;&emsp;nullable: false,<br>&emsp;&emsp;range: 1900 <= year <= This Year<br>} | {<br>&emsp;"stockid": 16,<br>&emsp;"make": "VW",<br>&emsp;"model": "tegruan",<br>&emsp;"year": 2012<br>} |


# API Output Usage
The Output of the API would be in json Detail sample in success return as below
| HTTP Method    | URI                  | Output description                                             | Exmaple          |
|----------------|----------------------|----------------------------------------------------------------|------------------|
| POST           | /api/Dealer/Login    | Login jwt token                                                | {<br>&emsp;"token": ".SgqndLn-yRfCatNIWq3VbF5TAaskzomC7jHsqGqr2TA",<br>&emsp;"dealer_id": 3,<br>&emsp;"expired_at": "2024-10-05 08:33:27"<br>}|
| POST           | /api/Dealer/Register | Login jwt token                                                | {<br>&emsp;"token": ".SgqndLn-yRfCatNIWq3VbF5TAaskzomC7jHsqGqr2TA",<br>&emsp;"dealer_id": 3,<br>&emsp;"expired_at": "2024-10-05 08:33:27"<br>}|
| GET            | /api/Dealer          | logged in dealer object json                                   | {<br>&emsp;"dealername": "rose",<br>&emsp;"dealeremail": "rose@testmail.com"<br>} |
| PUT            | /api/Dealer          | Update logged in dealer object json                            | {<br>&emsp;"dealername": "rose",<br>&emsp;"dealeremail": "rose@testmail.com"<br>} |
| GET            | /api/Stock           | Car stock json array that owned by dealer                      | [<br>&emsp;{<br>&emsp;&emsp;"stockid": 8,<br>&emsp;&emsp;"dealerid": 3,<br>&emsp;&emsp;"make": "audi",<br>&emsp;&emsp;"model": "A5",<br>&emsp;&emsp;"year": 2010,<br>&emsp;&emsp;"price": 10000,<br>&emsp;&emsp;"quantity": 20<br>&emsp;},<br>&emsp;{<br>&emsp;&emsp;"stockid": 9,<br>&emsp;&emsp;"dealerid": 3,<br>&emsp;&emsp;"make": "audi",<br>&emsp;&emsp;"model": "Q5",<br>&emsp;&emsp;"year": 2012,<br>&emsp;&emsp;"price": 8500,<br>&emsp;&emsp;"quantity": 10<br>&emsp;}<br>] |
| POST           | /api/Stock           | Newly added complete car stock json                            | {<br>&emsp;"stockid": 16,<br>&emsp;"dealerid": 3,<br>&emsp;"make": "VW",<br>&emsp;"model": "tegruan",<br>&emsp;"year": 2013,<br>&emsp;"price": 3000,<br>&emsp;"quantity": 1<br>}  |
| DELETE         | /api/Stock           | Nothing return, in status code 200                             | - |
| GET            | /api/Stock/search    | Car stock json array that owned by dealer and match the search | [<br>&emsp;{<br>&emsp;&emsp;"stockid": 8,<br>&emsp;&emsp;"dealerid": 3,<br>&emsp;&emsp;"make": "audi",<br>&emsp;&emsp;"model": "A5",<br>&emsp;&emsp;"year": 2010,<br>&emsp;&emsp;"price": 10000,<br>&emsp;&emsp;"quantity": 20<br>&emsp;},<br>&emsp;{<br>&emsp;&emsp;"stockid": 9,<br>&emsp;&emsp;"dealerid": 3,<br>&emsp;&emsp;"make": "audi",<br>&emsp;&emsp;"model": "Q5",<br>&emsp;&emsp;"year": 2012,<br>&emsp;&emsp;"price": 8500,<br>&emsp;&emsp;"quantity": 10<br>&emsp;}<br>] |
| POST           | /api/Stock/AdjQty    | Updated complete car stock json                                |{<br>&emsp;"stockid": 16,<br>&emsp;"dealerid": 3,<br>&emsp;"make": "VW",<br>&emsp;"model": "tegruan",<br>&emsp;"year": 2013,<br>&emsp;"price": 3000,<br>&emsp;"quantity": 3<br>} |
| POST           | /api/Stock/AdjPrice  | Updated complete car stock json                                | {<br>&emsp;"stockid": 16,<br>&emsp;"dealerid": 3,<br>&emsp;"make": "VW",<br>&emsp;"model": "tegruan",<br>&emsp;"year": 2013,<br>&emsp;"price": 2500,<br>&emsp;"quantity": 3<br>} |
| POST           | /api/Stock/AdjInfo   | Updated complete car stock json                                | {<br>&emsp;"stockid": 16,<br>&emsp;"dealerid": 3,<br>&emsp;"make": "VW",<br>&emsp;"model": "tega2",<br>&emsp;"year": 2013,<br>&emsp;"price": 2500,<br>&emsp;"quantity": 3<br>} |

# API Output Exception Handle
The API should return in json or nothing in HTTP Status Code 200 if in succeed. Otherwise, it should return in 400 Bad request for handled error or 401 unauthorized for invalid token. System exception would return in 500 server error as reference.
