# CheckPoint

CheckPoint gives everyday users a Git client for their basic backup and restore needs.

By using a single branch workflow, we keep things simple while still benefiting from Git delta 
versioning and compression. This gives users a more effient alternative to traditional ZIP 
archiving.

Features include:
* Windows Desktop GUI. 
* Create local Git repos.
* Manage a library of repos.
* Commit and log your latest changes with and automated Backup function.
* Revert to a previous commit with an automated Restore function.


## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. 

See deployment for notes on how to deploy the project on a live system.

### Prerequisites

* Microsoft Visual Studio 2017
* .NET Framework v4.7.2

### Installing

1. Clone or download the latest stable or development branch.

```
https://github.com/onyx-digital/checkpoint.git
```

2. Open the solution using MS Visual Studo.

```
checkpoint/CheckPoint.sln
```

## Deployment

The solution includes a Windows Installer for simple automated installation.

1. Build the Installer Project.

2. Copy the install the package installer to the target system.

```
CheckPoint.Installer/Release/setup.exe
```

## Built With

* [MahApps](https://mahapps.com/) - WPF Styling
* [NLog](https://nlog-project.org/) - Logging
* [LibGit2Sharp](https://github.com/libgit2/libgit2sharp) - Git Client Library

## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on our code of conduct, and the process for submitting pull requests to us.

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/onyx-digital/checkpoint/tags). 

## Authors

* **Chris OBeirne** - *Initial work* - [Onyx Digital](https://github.com/onyx-digital)

See also the list of [contributors](contributors.md) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE) file for details

## Acknowledgments

* **Tom OBeirne** - *Product Design* - [Onyx Digital](https://github.com/onyx-digital)

