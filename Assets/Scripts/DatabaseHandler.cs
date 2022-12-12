using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullSerializer;
using Proyecto26;

public class DatabaseHandler
{
    private static string levelname = LevelAndGenreManager.levelName;
    private const string projectId = "timetokill-1bdbd"; 
    private static readonly string databaseURL = $"https://{projectId}-default-rtdb.firebaseio.com/";
    
    private static fsSerializer serializer = new fsSerializer();

    public delegate void PostUserCallback();
    public delegate void GetUserCallback(User user);
    public delegate void GetUsersCallback(Dictionary<string, User> users);

    public static void PostUser(User user, string levelName, string userId, PostUserCallback callback)
    {
        RestClient.Put<User>($"{databaseURL}{levelName}/{userId}.json", user).Then(response => { callback(); });
    }

    public static void GetUser(string userId, string levelName, GetUserCallback callback)
    {
        RestClient.Get<User>($"{databaseURL}{levelName}/{userId}.json").Then(user => { callback(user); });
    }

    public static void GetUsers(string levelName, GetUsersCallback callback)
    {
        RestClient.Get($"{databaseURL}{levelName}.json").Then(response =>
        {
            var responseJson = response.Text;

            // Using the FullSerializer library: https://github.com/jacobdufault/fullserializer
            // to serialize more complex types (a Dictionary, in this case)
            var data = fsJsonParser.Parse(responseJson);
            object deserialized = null;
            serializer.TryDeserialize(data, typeof(Dictionary<string, User>), ref deserialized);

            var users = deserialized as Dictionary<string, User>;
            callback(users);
        });
    }
}
