# TopStoriesAPI
RESTful API .NET 8 project using ASP.NET Core Web API, MemoryCache, HackerNews API, Docker, NUnit, Moq

## Overview
This repository contains solution to developer coding test:

>Using ASP.NET Core, implement a RESTful API to retrieve the details of the best n stories from the Hacker News API, as determined by their score, where n is specified by the caller to the API.

The API should return an array of the best n stories as returned by the Hacker News API in descending order of score, in the form:

```JSON
[
	{
		"title": "A uBlock Origin update was rejected from the Chrome Web Store",
		"uri": "https://github.com/uBlockOrigin/uBlock-issues/issues/745",
		"postedBy": "ismaildonmez",
		"time": "2019-10-12T13:43:01+00:00",
		"score": 1716,
		"commentCount": 572
	},
	{ ... },
	{ ... },
	{ ... },
	...
]
```

>In addition to the above, your API should be able to efficiently service large numbers of requests without risking overloading of the Hacker News API.

## Observations
- HackerNews API have no limits for number of calls so there is no need to implement any throttling as per: [documentation](https://github.com/HackerNews/API#uri-and-versioning)
- [`/beststories`](https://hacker-news.firebaseio.com/v0/beststories.json) always returns collection of identifiers for top 200 stories based on their score in descending order
- There is no way to subscribe for any changes
- Story score can change in time between call to `/beststories` and calls for each individual story `/item/{id}`

## Solution

## How to run
### VisualStudio:

### DotNetCLI:

---
## Possible improvements
- Check 