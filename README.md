# PromoForwarder
The console application uses Post Office Protocol Version 3 to connect to the user's mail, reads the latest emails and forwards those that match the regular expression to another mail. The Quartz.NET library is also used to run the application at a certain interval.

## Executing program
Open the App.config file and specify valid data. To change the regular expression by which emails are selected, pass the third parameter to the constructor of the EmailReader class - your regular expression.

## License
This project is licensed under the MIT License - see the LICENSE.md file for details.
