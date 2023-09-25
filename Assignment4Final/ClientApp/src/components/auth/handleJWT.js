// import { authenticationResponse, claim } from './auth.models';
//mallon axrista
import { useNavigate } from "react-router-dom";

const tokenKey = "token";
const expirationKey = "token-expiration";

export function saveToken(authData) {
  localStorage.setItem(tokenKey, authData.token);
  localStorage.setItem(expirationKey, authData.expiration.toString());
}

export function getClaims() {
  const token = localStorage.getItem(tokenKey);

  if (!token) {
    return [];
  }

  const expiration = localStorage.getItem(expirationKey);
  const expirationDate = new Date(expiration);

  if (expirationDate <= new Date()) {
    logout();
    return []; // the token has expired
  }

  const dataToken = JSON.parse(atob(token.split(".")[1]));
  const response = [];
  for (const property in dataToken) {
    response.push({ name: property, value: dataToken[property] });
  }

  return response;
}

export function getUserId(token) {
  const dataToken = JSON.parse(atob(token.split('.')[1]));
  const claims = [];
  for (const property in dataToken) {
    claims.push({ name: property, value: dataToken[property] });
  }
  if (claims.length > 0) {
    return claims.find(claim => claim.name === 'userId').value
  }
  return ""
}

export function logout(navigate) {
  localStorage.removeItem(tokenKey);
  localStorage.removeItem(expirationKey);
  window.location.reload();
  window.location.href = "/"
}

export function clean() {
  localStorage.removeItem(tokenKey);
  localStorage.removeItem(expirationKey);

}

export function getToken() {
  return localStorage.getItem(tokenKey);
}
