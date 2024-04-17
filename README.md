# MSAL Demos
this is a simple demo (might eventually be a set of demos) of MSAL. A few notes:
- This is far from best practices. I've used MSAL a lot, however, I do not write code for a living. THis is also MIT licensed, which means, if this burns your computer, delete all your files, and breaks your car engine, I won't be liable.
- You might have gotten here for a multitude of reasons. Under no circunstance however, should this be considered a Microsoft-backed repo. This is 100% personal, MIT licensed, and to be used as an example. No support will be given.


Having said that, this proyect contains:
- A simple app that loads all the users and all the devices on a given tenant, using the Graph API, authenticated with MSAL.
- A simple secrets management (plain text storage in "C:\SecretsStorage" so I don't have to push them to GitHub.
- Windows credentials caching so you don't have to type them every time.
- Windows accounts integration so you might as well never have to type your credentials at all.
- A set of helpers, including Users and Devices objects helpers. 
