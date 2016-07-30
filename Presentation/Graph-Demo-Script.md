
# Setup

## 1.      Open OneDrive for Business with AAD Credentials (from http://office.com)

## 2.      Create a new Excel Workbook

## 3.      Open Graph Explorer

## 4.      Split display so you can see the workbook and graph explorer

## 5.      Login with AAD credentials

# Excel

## 1.      Find the workbook you created

<div style="background: black; padding: 1pt 4pt; border: 1pt solid windowtext; border-image: none;">

GET

/beta/me/drive/root/children?$select=name,id


## 2.      Get a reference to the workbook without the excel reference (file only)

<div style="background: black; padding: 1pt 4pt; border: 1pt solid windowtext; border-image: none;">

GET

/beta/me/drive/items/[id]


## 3.      Now get a reference with the excel reference

<div style="background: black; padding: 1pt 4pt; border: 1pt solid windowtext; border-image: none;">

GET

/beta/me/drive/items/[id]/workbook


## 4.      List the sheets

<div style="background: black; padding: 1pt 4pt; border: 1pt solid windowtext; border-image: none;">

GET

/beta/me/drive/items/[id]/workbook/worksheets


## 5.      Get a reference to Sheet1

<div style="background: black; padding: 1pt 4pt; border: 1pt solid windowtext; border-image: none;">

GET

/beta/me/drive/items/[id]/workbook/worksheets('Sheet1')


## 6.      Retrieve the contents of the range Sheet1!A1:D1

<div style="background: black; padding: 1pt 4pt; border: 1pt solid windowtext; border-image: none;">

GET

/beta/me/drive/items/[id]/workbook/worksheets('Sheet1')/range(address='Sheet1!A1:D1')


## 7.      Write to the range Sheet1!A1:C1

<div style="background: black; padding: 1pt 4pt; border: 1pt solid windowtext; border-image: none;">

PATCH

/beta/me/drive/items/[id]/workbook/worksheets('Sheet1')/range(address='Sheet1!A1:D1')

{

    "values" : [["Given Name", "Family Name", "Company", "Children"]]

}


## 8.      Create a table based on the headers we just added

<div style="background: black; padding: 1pt 4pt; border: 1pt solid windowtext; border-image: none;">

POST

/beta/me/drive/items/[id]/workbook/worksheets('Sheet1')/tables/$/add

{

    "address" : "Sheet1!A1:D1",

    "hasheaders" : true

}


## 9.      List the tables

<div style="background: black; padding: 1pt 4pt; border: 1pt solid windowtext; border-image: none;">

GET

/beta/me/drive/items/[id]/workbook/worksheets('Sheet1')/tables


## 10\. Add a row to the table

<div style="background: black; padding: 1pt 4pt; border: 1pt solid windowtext; border-image: none;">

POST

/beta/me/drive/items/[id]/workbook/worksheets('Sheet1')/tables('Table1')/rows

{

   "values" : [["Bill", "Gates", "Microsoft", 3]]

}


## 11\. And another

<div style="background: black; padding: 1pt 4pt; border: 1pt solid windowtext; border-image: none;">

POST

/beta/me/drive/items/[id]/workbook/worksheets('Sheet1')/tables('Table1')/rows

{

    "values" : [["Elon", "Musk", "SpaceX", 6]]

}


## 12\. This fails for adding multiple rows to a table (for now)

<div style="background: black; padding: 1pt 4pt; border: 1pt solid windowtext; border-image: none;">

POST

/beta/me/drive/items/[id]/workbook/worksheets('Sheet1')/tables('Table1')/rows

{

   "values" : [["Jeff", "Bezos", "Amazon", 4],["Eric", "Schmidt", "Google", 2]]

}


## 13\. But this works

<div style="background: black; padding: 1pt 4pt; border: 1pt solid windowtext; border-image: none;">

PATCH

/beta/me/drive/items/[id]/workbook/worksheets('Sheet1')/range(address='Sheet1!A4D5')

{

   "values" : [["Jeff", "Bezos", "Amazon", 4],["Eric", "Schmidt", "Google", 2]]

}


## 14\. Create a chart

<div style="background: black; padding: 1pt 4pt; border: 1pt solid windowtext; border-image: none;">

POST

/beta/me/drive/items/[id]/workbook/worksheets('Sheet1')/charts/$/add

{

  "type": "Pie",

  "sourceData": "C1:D5",

  "seriesBy": "Auto"

}


## 15\. Get the chart image

<div style="background: black; padding: 1pt 4pt; border: 1pt solid windowtext; border-image: none;">

GET

/beta/me/drive/items/[id]/workbook/worksheets('Sheet2')/charts('chart 1')/image


Show the Base64 encoded string that comes back

# Groups

## 1.      All Groups

<div style="background: black; padding: 1pt 4pt; border: 1pt solid windowtext; border-image: none;">

GET

/beta/groups?$select=id,displayName,description


## 2.      My Groups

<div style="background: black; padding: 1pt 4pt; border: 1pt solid windowtext; border-image: none;">

GET

/beta/me/memberof?$select=id,displayName,description


Note the additional group – it’s not a unified group

## 3.      Get Group Detail (members, image etc)

<div style="background: black; padding: 1pt 4pt; border: 1pt solid windowtext; border-image: none;">

GET

/beta/groups/[id]


## 4.      Create Group

<div style="background: black; padding: 1pt 4pt; border: 1pt solid windowtext; border-image: none;">

POST

/beta/groups

{

  "displayName": "Jetsetters",

  "description": "If you travel around a lot, this group is for you",

  "groupTypes": [ "Unified" ],

  "mailEnabled": true,

  "mailNickname": "jetsetters",

  "securityEnabled": false

}


## 5.      Group Members

<div style="background: black; padding: 1pt 4pt; border: 1pt solid windowtext; border-image: none;">

GET

/beta/groups/[id]/members


## 6.      Get Group Files

<div style="background: black; padding: 1pt 4pt; border: 1pt solid windowtext; border-image: none;">

GET

/beta/groups/[id]/drive/root/children


## 7.      Get Group Conversations

<div style="background: black; padding: 1pt 4pt; border: 1pt solid windowtext; border-image: none;">

GET

/beta/groups/[id]/conversations


## 8.      Get Group Calendar Events

<div style="background: black; padding: 1pt 4pt; border: 1pt solid windowtext; border-image: none;">

GET

/beta/groups/[id]/events


## 9.      Get Group Plans

<div style="background: black; padding: 1pt 4pt; border: 1pt solid windowtext; border-image: none;">

GET

/beta/groups/[id]/plans


_etcetera_
