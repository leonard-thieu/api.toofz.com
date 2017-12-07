# toofz API

[![Build status](https://ci.appveyor.com/api/projects/status/2en9f6hcf72ujm9y/branch/master?svg=true)](https://ci.appveyor.com/project/leonard-thieu/toofz-necrodancer-web-api/branch/master)
[![codecov](https://codecov.io/gh/leonard-thieu/api.toofz.com/branch/master/graph/badge.svg)](https://codecov.io/gh/leonard-thieu/api.toofz.com)

## Overview

[**toofz API**](https://api.toofz.com/) is a REST API that serves data for [Crypt of the NecroDancer](http://necrodancer.com/) items, enemies, leaderboards, and player stats. Its primary purpose 
is to serve as a backend for [toofz](https://crypt.toofz.com/). It also serves other community projects such as [StatsBot](https://github.com/necrommunity/Statsbot).

**toofz API** is an ASP.NET Web API-based service. It uses Entity Framework to retrieve data from an MS SQL Server database.

---

**toofz API** is a component of **toofz**. 
Information about other projects that support **toofz** can be found in the [meta-repository](https://github.com/leonard-thieu/toofz-necrodancer).

### Dependents

* [toofz](https://github.com/leonard-thieu/crypt.toofz.com)

### Dependencies

* [toofz Leaderboards Service](https://github.com/leonard-thieu/leaderboards-service)
* [toofz Players Service](https://github.com/leonard-thieu/players-service)
* [toofz Replays Service](https://github.com/leonard-thieu/replays-service)
* [toofz NecroDancer Core](https://github.com/leonard-thieu/toofz-necrodancer-core)
* [toofz Data](https://github.com/leonard-thieu/toofz-data)

## Requirements

* .NET Framework 4.6.1
* IIS
* MS SQL Server

## License

**toofz API** is released under the [MIT License](LICENSE).