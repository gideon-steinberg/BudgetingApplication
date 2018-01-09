# Budgeting Application

A simple windows exe application that allows you to see cash flow in a business.

See the Example.png to see how it looks.

## How do I use it?

Simply compile the application with Visual Studio and run the application.

There is an expected.csv which contains all your expected payments and income.

You can either use the inbuilt editor (which checks for valid formatting) or manually update the csv.

The program will crash on load if the csv is mal formatted!

## Expense types and examples

Things happening once a month on the 20th of the month : `Test,-200,Montly,20`

Things happening on a specific date: `Test,-200,Yearly,1 April`

Things happening every day: `Income,1000,Daily,1`

Things happening on a specific day of the week: `Test,-200,Weekly,Tuesday`

Things happening at the final day of the month: `Test,-200,Monthly,EndOfMonth`

Things happening every second month on the 20th of the month: `Test,-200,BiMonthly,20`

Note - GST on a BiMonthly basis will make the December payment happen in January for reasons.

## Disclaimer

This program was written during my time off and is not written in a pretty way. It gets the job done but it is messy.