import axios from "axios";
import { getToken } from "./handleJWT";

export default function configureInterceptor() {
    axios.interceptors.request.use(
        (config) => {
            config.headers["Authorization"] = `Bearer ${getToken()}`;
            config.withCredentials = true;
            return config;
        },
        (error) => {
            return Promise.reject(error);
        }
    );
}
