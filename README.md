# Disclaimer

This tool is made available for the convenience of the Washington Community and
Technical College System. Bellevue College does not support, warranty or assist
in the use of the tool.

# Project Overview

The goal of this project is to provide an auto generated file (based on the
given configurations) that act as an input to 25Live software, which will be
used to automate the process of scheduling rooms on campus.

## Software

* Microsoft .Net Framework 4.5
* IIS 7.5
* Visual Studio 2013
    * C#
    * EntityFramework 6.0

## Feature Set

   1. Use a URL format like for eg: http://domainname/../CreateDatain/CreateNewFile/B561 to run the script that creats Datain       file named as defined in the config concatenated with the Year Quarter.
   1. You can change the yearQuarter in the url above to create datain for a particular quarter.
   1.	Move existing file into Archive (rename it to originalName+Timestamp).
   1.	Scheduled task to automatically run the application for a given quarter.

## Configuration

Please refer web.config-template file.
