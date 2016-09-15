[![Build Status](https://travis-ci.org/abhurtun/PaypalService.svg?branch=master)](https://travis-ci.org/abhurtun/PaypalService)

# Docker ASP.NET WebAPI Example
This repo is an example of how to run an ASP.NET WebAPI solution in Docker.

## Documentation
Documentation for this repository can be found here: TBC

## Curl Test Commands
The following is a list of curl commands that can be used to test the API once it is running.

*Make sure to fill out the values listed in greater than and less than signs.  Endpoint will depend upon your running environment.  In my dev machine, the endpoint generated for my VirtualBox host is 192.168.99.100.*

**Test Endpoint:**

Run the following command to use curl to create a new order.
```bash
curl --data '{"FirstName": "Chris", "LastName": "Myers", "Address": {"Street1": "123 Abc St", "City": "Phoenix", "State": "AZ", "Zip": "123456"}, "ItemId": "21a0276a-ff97-4d5a-828b-ae13024f4aec", "Quantity": 5}' 

http://<endpoint>:5001/api/order
```

**For Docker**

Run the following command to build and run the docker image.
```bash
- docker build -t paypal .
- docker run -p 5001:5001 -i paypal

http://192.168.99.100:5001/api/checkouts/
```
