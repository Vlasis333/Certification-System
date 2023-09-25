import axios from "axios";
import { getToken } from './handleJWT';


// axios.defaults.headers.common['Authorization'] = `Bearer ${getToken()}`;


// export default function configureInterceptor() {
//   axios.interceptors.request.use(
//       (config) => {
//           config.headers["Authorization"] = `Bearer ${getToken()}`;
//           return config;
//       },
//       (error) => {
//           return Promise.reject(error);
//       }
//   );
// }
const instance = axios.create({
  baseURL: 'http://127.0.0.1:8000/api/',
  headers: {
    // accept: 'text/plain',
    Authorization: `Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6ImNhbmRpZGF0ZThAZ21haWwuY29tIiwidXNlcklkIjoiMTMxMjU1NzYtZjg3My00M2Q0LTgxMDMtN2JjY2RkYjUxMzUxIiwicm9sZSI6ImNhbmRpZGF0ZSIsImV4cCI6MTcwNzQzMzY5MX0.ls87n2t1IDjcEEamBdzs2d0u5H8FQu8pn5JBKis2JbI`,
    // Bearer: ` ${getToken()}`,
    // 'Content-Type': "application/json",
}
    // .. other options
  });

export default instance;
//     axios.interceptors.request.use(
//         function (config){
//             const token = getToken();

//             if (token){
//                 config.headers.Authorization = `Bearer ${token}`;
//             }

//             return config;
//         },
//         function (error){
//             return Promise.reject(error);
//         }


