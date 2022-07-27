# Unofficial InstaAPI

It was created for educational purposes only.

A reverse-engineered implementation of the [Instagram](https://instagram.com/) web app's API.

**NOTE:** I can't guarantee you will not be blocked by using this method, although it has worked for me. Instagram does not allow bots or unofficial clients on their platform, so this shouldn't be considered totally safe.

## Quick Links

* [Discord](https://discord.gg/sYeya7g)
* [GitHub](https://github.com/higordiasz/InstaAPI)
* [Nuget](https://www.nuget.org/packages/InstaAPI/1.0.0)

## Installation

The module is now available on nuget!

``dotnet add package InstaAPI --version 1.0.0``

## Example usage

```c#
using InstaAPI;
using InstaAPI.Profile.Follow;
using InstaAPI.Profile.Login;
using InstaAPI.Model.Return;

namespace InstaApiUsage
{
  class Program
  {
    static async Task Main()
    {
      Instagram insta = new("username", "password");
      IReturn login = await insta.Login();
      if (login.Status == 1)
      {
        IReturn profileId = await insta.GetIdBySearchBar("cristiano");
        if (profileId.Status == 1)
        {
          IReturn follow = await insta.FollowUserById(profileId.Response);
          if (follow.Status == 1)
          {
            Console.WriteLine("Success to follow user");
          }
          else
          {
            Console.WriteLine(login.Response);
          }
        }
        else
        {
          Console.WriteLine(login.Response);
        }
      }
      else
      {
        Console.WriteLine("Failed to login");
        Console.WriteLine(login.Response);
      }
    }
  }
}
```

## Supported features

| Functions  | Status | Description |
| ------------- | ------------- | ------------- |
| Login | ✅  | Login to instagram account |
| FollowUserById  | ✅  | Follow user by userId |
| GetRelationById  | ✅  | Get relation of you account and target userId |
| GetIdBySearchBar  | ✅  | Find user by search bar of Instagram |
| GetGender  | ✅ | Get gender of account |
| GetUserProfileFromUsername | ✅ | Get user profile information using Username |
| SeeStorie  | ✅  | See stories witch storiesId |
| SendStoriesLike | ✅ | Like stories witch storiesId |
| CommentMediaByMediaId | ✅ | Comment media |
| LikeMediaByMediaId | ✅ | Like media |
| StoriesFeedClass | ✅ | Get a list of stories |
| GetMyInbox | ✅ | Get account inbox notifications |
| SeeMyInbox | ✅ | See account inbox notifications |

Something missing? Make an issue and let us know!

## Contributing

Pull requests are welcome! If you see something you'd like to add, please do. For drastic changes, please open an issue first.

## Supporting the project

You can support the maintainer of this project through the links below

- [Support via GitHub Sponsors](https://github.com/sponsors/higordiasz)

## Disclaimer

This project is not affiliated, associated, authorized, endorsed by, or in any way officially connected with Instagram or any of its subsidiaries or its affiliates. The official Instagram website can be found at https://instagram.com. "Instagram" as well as related names, marks, emblems and images are registered trademarks of their respective owners.

## License

Copyright 2022 Higor D Zuqueto

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this project except in compliance with the License.
You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
