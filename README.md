# Histogram chart with data from datbase

![Review](https://img.shields.io/badge/Project%20Review-1.0-informational) ![FTOptix](https://img.shields.io/badge/FTOptix%20Version-1.3.2.3-blue) ![Tested](https://img.shields.io/badge/Tested-Yes-green)

This project contains a histogram graph typed with a netlogic and provides visualisation of the database data. This also allows you to create a query with AVG and SUM and display the results natively with an object simply to instantiate and configure.

This example has a query that is dynamised with a SpinBox to retrieve data from a production using an ID as the search key.

## Setup

Drop the HistogramChartDatabaseData type into UI folder, now create an instance of this object into your screen and configure the parameters:

- Link your *store* to the NodePointer **Database**
- Write or link the query to the variable **QueryString**

## Notes

- Label is the name of colums, if you want a new implementation for customize the labels, please open a Git Issue on this project
- Only the first row of the query will be shown
