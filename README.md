# Before start

Please, read about [Polimaster repositories](https://github.com/polimaster/.github-private).
Here is important information about repositories, general rules, software or hardware release cycles
and other useful information.


# Repository template

This is default repository template for Polimaster projects.

For new repository replace content of this file with appropriate information such as
project description, reference links, instructions etc. This information should
uniquely identify the project and its purpose.

# Template usage

For new repositories:

- use [rules](https://github.com/polimaster/.github-private) while creating repository
- go to [template repository](https://github.com/polimaster/pm-gen-repository-template)
- select `Use this template` > `Create new repository`
- alter [issue templates](./.github/ISSUE_TEMPLATE) for your new repository
- prepare github workflows and actions

For existing repositories:

- replicate directory / files structure of template

For additional information refer to [Creating a repository from a template](https://docs.github.com/en/repositories/creating-and-managing-repositories/creating-a-repository-from-a-template)

# Repository structure

## Documents

[Documents directory](./doc) should contains project documentation including 
technical requirements and specifications, guides and installation instructions for developer.
All documentation should be actual for certain project release version 
(see [release cycle](./doc/release-cycle.md)).

> Note. Please, abstain from using binary files such as (.doc). Use markdown files (.md) instead.


## Documents source code

[Documents source code directory](./doc-src) should contain source code for end-user documentation generation.
On example, software and hardware manuals, API documentation, instructions etc.  

Documents source code is a set of markdown files which can be transformed to pdf/html.

> Note: think about transforming project source code comments to API documentation.


### Useful links

- [Markdown to PDF](https://github.com/BaileyJM02/markdown-to-pdf) to generate pdf files from markdown.



## Source code

[Source code directory](./src) is a directory for storing project source code.


# What else?

- GitHub [documentation](https://docs.github.com/en)
- GitHub Actions [Documentation](https://docs.github.com/en/actions)
- [How to create .NET Core release](https://patriksvensson.se/posts/2020/03/creating-release-artifacts-with-github-actions) artifacts with GitHub Actions.