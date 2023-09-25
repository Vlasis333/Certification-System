import React, { useState, useContext } from 'react';
import { useNavigate } from "react-router-dom";
import { Form, Button, Stack } from 'react-bootstrap';
import { AuthenticationContext } from '../auth/AuthenticationContext'
import { getClaims, saveToken, clean } from './handleJWT'
import ErrorsRegister from '../Common/ErrorListRegister'
import axios from 'axios';

const RegisterForm = () => {
  const navigate = useNavigate();
  const { update } = useContext(AuthenticationContext);
  const [error, setError] = useState(null);

  const [user, setUser] = useState({
    // Id: '',
    UserName: '',
    Email: '',
    // PhoneNumber: '',
    // Role: '',
    Credentials: {
      Email: '',
      Password: '',
      IsCandidate: true
    }
  });

  const handleChange = (event) => {
    const target = event.target;
    const name = target.name;
    const value = target.value;
    if (name === 'Email') {
      setUser({
        ...user, [name]: value,
        Credentials: {
          ...user.Credentials, [name]: value,
        },
      });
    } else {
      setUser({ ...user, [name]: value });
    }
  };

  const handleCredentialsChange = (event) => {
    const target = event.target;
    const name = target.name;
    const value = target.type === 'checkbox' ? target.checked : target.value;
    setUser({
      ...user,
      Credentials: {
        ...user.Credentials,
        [name]: value
      }
    });
  };

  const handleSubmit = async (event) => {
    event.preventDefault();

    await axios.post(`https://localhost:7196/api/accounts/create`, user)
      .then(response => {
        console.log(response);
        update([]);
        // saveToken(response.data);
        // update(getClaims());
        navigate("/login"); 
        setError([]);

        if (user.Credentials.IsCandidate === true) {
          // navigate("/candidate/create");
        }
        else {
          navigate("/");
        }
      })
      .catch(error => {
        console.error(error);
        setError(error);
      });
  };

  return (

    <div className="d-grid justify-content-center lead">
      {error && <ErrorsRegister error={error} />}
      <Stack gap={4}>

        <h3 className="display-6">Register</h3>


        <Form onSubmit={handleSubmit}>
          <Stack gap={2}>

            <Form.Group controlId="formBasicEmail">
              <Form.Label>Email address</Form.Label>
              <Form.Control
                type="email"
                placeholder="Enter email"
                name="Email"
                value={user.Email}
                onChange={handleChange}
              />
            </Form.Group>
            <Form.Group controlId="formBasicUserName">
              <Form.Label>Username</Form.Label>
              <Form.Control
                type="text"
                placeholder="Enter username"
                name="UserName"
                value={user.UserName}
                onChange={handleChange}
              />
            </Form.Group>

            <Form.Group controlId="formBasicCredentialsPassword">
              <Form.Label>Password</Form.Label>
              <Form.Control
                type="password"
                placeholder="Password"
                name="Password"
                value={user.Credentials.Password}
                onChange={handleCredentialsChange}
              />
            </Form.Group>
            <Form.Group controlId="formBasicCredentialsIsCandidate" hidden>
              <Form.Check
                type="checkbox"
                label="Register As Candidate?"
                name="IsCandidate"
                checked={user.Credentials.IsCandidate}
                onChange={handleCredentialsChange}
              />
            </Form.Group>
            <Button className="mt-2" variant="primary" type="submit">
              Submit
            </Button>
          </Stack>
        </Form>
      </Stack>

    </div>
  );
};

export default RegisterForm;
