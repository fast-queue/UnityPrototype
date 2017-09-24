using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace FQ {

    public class RestApi {
        private string key;
        private string baseUrl;

        public RestApi () { }

        public RestApi (string url, string key) {
            this.baseUrl = url;
            this.key = key;
        }

        public T deleteQueue<T> (T queue) where T : FQ.BaseBody {
            var url = baseUrl + "/queue/" + queue._id;
            return this.delete<T> (url);
        }

        public T deletePlayer<T, K> (T queue, K player) where T : FQ.BaseBody
                                                        where K : FQ.BaseBody {
            var url = baseUrl + "/queue/" + queue._id + "/players/" + player._id;
            return this.delete<T> (url);
        }

        public T addQueue<T> (T obj) where T : FQ.BaseBody {
            var url = baseUrl + "/queue";
            return add (url, obj);
        }

        public T[] getAllQueue<T> (){
            var url = baseUrl + "/queue";
            var queues = get<T> (url);
            if (queues.Length > 0)
                return queues;
            return new T[0];
        }

        public T[] getAllQueue<T> (string plugin) {
            var url = baseUrl + "/queue?callback=" + plugin;
            var queues = get<T> (url);
            if (queues.Length > 0)
                return queues;
            return null;
        }

        public T getQueue<T> (T queue) where T : FQ.BaseBody {
            var url = baseUrl + "/queue/" + queue._id;
            return getOne<T> (url);
        }

        public K[] getPlayers<T, K> (T queue) where T : FQ.BaseBody
                                              where K : FQ.BaseBody {
            var url = baseUrl + "/queue/" + queue._id + "/players";
            return get<K> (url);
        }

        public K[] getPlayers<T, K> (T queue, string plugin) where T : FQ.BaseBody
                                                             where K : FQ.BaseBody {
            var url = baseUrl + "/queue/" + queue._id + "/players";            
            if (plugin != null) {
                url += "?callback=" + plugin;
            }
            return get<K> (url);
        }

        public K getPlayer<T, K> (T queueId, K playerId) where T : FQ.BaseBody 
                                                         where K : FQ.BaseBody {
            var url = baseUrl + "/queue/" + queueId._id + "/players/" + playerId._id;
            return getOne<K> (url);
        }

        public K getPlayer<T, K> (T queue, K player, string plugin) where T : FQ.BaseBody 
                                                                        where K : FQ.BaseBody{
            var url = baseUrl + "/queue/" + queue._id + "/players/" + player._id + "?callback=" + plugin;
            return getOne<K> (url);            
        }

        public K addPlayer<T, K> (T queue, K player) where T : FQ.BaseBody
                                                     where K : FQ.BaseBody {
            var url = baseUrl + "/queue/" + queue._id + "/players";
            return add<K> (url, player);
        }

        private T add<T> (string url, T obj) where T : FQ.BaseBody {
            T retObj = obj;
            var type = RequestType.Post;
            if (obj == null) {
                throw new Exception ("No object recived on post!");
            }
            var sendObj = objectToJSON (obj);
            var request = this.Send (url, type, sendObj);
            T response = convertResponse<T> (request);
            retObj._id = response._id;
            return retObj;
        }

        private T delete<T> (string url) {
            var type = RequestType.Delete;
            var request = this.Send (url, type, null);
            var response = convertResponse<T> (request);
            return response;
        }

        private T[] get<T> (string url) {
            var type = RequestType.Get;
            var request = this.Send (url, type, null);
            var x = "{ objects:" + request + "}";
            var response = convertResponse<getAllToWork<T>> (x);
            return response.objects;
        }

        private T getOne<T> (string url) {
            var type = RequestType.Get;
            var request = this.Send (url, type, null);
            var response = convertResponse<T> (request);
            return response;
        }

        private string Send (string url, RequestType apiRequestType, string body) {
            var request = HttpWebRequest.Create (new System.Uri (url));
            request.Headers["API-KEY"] = this.key;
            request.Timeout = 21000; //milliseconds
            if (apiRequestType == RequestType.Get) {
                request.Method = "GET";
            } else if (apiRequestType == RequestType.Post) {
                request.Method = "POST";
            } else if (apiRequestType == RequestType.Delete) {
                request.Method = "DELETE";
            } else if (apiRequestType == RequestType.Put) {
                request.Method = "PUT";
            }

            if (body != null) {
                var dataToSend = Encoding.UTF8.GetBytes (body);
                request.ContentType = "application/json";
                request.ContentLength = dataToSend.Length;
                request.GetRequestStream ().Write (dataToSend, 0, dataToSend.Length);
            }

            var response = request.GetResponse ();

            //HttpResponseMessage response = client.PostAsync(url, content).Result;
            using (var reader = new System.IO.StreamReader (response.GetResponseStream (), Encoding.UTF8)) {
                string responseString = reader.ReadToEnd ();

                return responseString;
            }
        }

        private T convertResponse<T> (string response) {
            //just trying to convert response to my helper empty dto, so i can understand if any error happened:)
            var body = JsonConvert.DeserializeObject<T> (response);
            if (body == null) {
                throw new Exception ("Erro conversão json - POST"); //throw error message
            }
            return body;
        }

        private string objectToJSON (object obj) {
            var serialized = JsonConvert.SerializeObject (obj);
            return serialized;
        }
    }
    class getAllToWork<T> {

        public T[] objects;
    }
}