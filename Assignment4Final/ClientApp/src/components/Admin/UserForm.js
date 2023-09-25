import React, { useEffect, useState, useContext } from "react";

import { ListGroup, ListGroupItem, Button, Table, Row, Col, Stack, Form, CloseButton } from 'react-bootstrap';
import { AuthenticationContext } from '../auth/AuthenticationContext'
import { getUserId } from '../auth/handleJWT'

import { useNavigate, useParams } from "react-router-dom";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import ErrorsRegister from '../Common/ErrorListRegister'

import axios from "axios";

function UserForm() {

  const params = useParams();
  const navigate = useNavigate();

  const [error, setError] = useState();
  const [user, setUser] = useState({});
  const [password, setPassword] = useState();
  const [confirmPassword, setConfirmPassword] = useState();
  const [errorMessage, setErrorMessage] = useState("");

  const [credentials, setCredentials] = useState({
    "email": "user@example.com",
    "password": "string",
    "newPassword": "string",
    "isCandidate": null
  });
  const [roles, setRoles] = useState([]);

  const fetchData = () => {
    if (params.email) {
      axios
        .get(`https://localhost:7196/api/accounts/getUser/${params.email}`)
        .then((response) => {
          setUser(response.data);
          // console.log(response.data)
        })
        .catch(function(error) {
          console.log(error);
        });
    }
    axios
      .get(`https://localhost:7196/api/accounts/getAllClaims`)
      .then((response) => {
        setRoles([...response.data]);
        // console.log(response.data)
      })
      .catch(function(error) {
        console.log(error);
      });

  };

  useEffect(() => {
    fetchData();
  }, []);
  const handleChange = (event) => {

    const { name, value, type } = event.target;
    //console.log(event.target)
    //console.log(name);
    //console.log(type);


    console.log(value)
    if (name === "newPassword2") {
      setConfirmPassword(value);
      if (value === password) {
        console.log("same!")
        setUser({
          ...user,
          credentials: {
            email: user.email,
            password: password
          }
        });
      } else {
        console.log("not same!")
        setUser({
          ...user,
          credentials: {
            email: user.email,
            password: null
          }
        });

      }
    } else if (name === "newPassword1") {
      setPassword(value);
      if (value === confirmPassword) {
        console.log("same!")
        setUser({
          ...user,
          credentials: {
            email: user.email,
            password: confirmPassword
          }
        });
      } else {
        console.log("not same!")
        setUser({
          ...user,
          credentials: {
            email: user.email,
            password: null
          }
        });
      }
    } else if (name === "email") {
      setUser({
        ...user, [name]: value,
        credentials: {
          email: value,
          password: confirmPassword
        }
      });
    }
    else {
      setUser({
        ...user, [name]: value
      })
    }
  }




  const handleSubmit = (event) => {
    event.preventDefault();
    // if (password !== confirmPassword) {
    //   console.log("Passwords do not match");
    // } else {
    //   console.log("I will send");


    if (params.email) {
      axios.put(`https://localhost:7196/api/accounts/update/${params.email}`, user)
        .then((response) => {
          console.log(response);
          navigate('/users')
        })
        .catch(function(error) {
          console.log(error);
          setError(error)
        });
    }
    else {
      axios.post(`https://localhost:7196/api/accounts/create`, user)
        .then((response) => {
          console.log(response);
          navigate('/users')
        })
        .catch(function(error) {
          console.log(error);
          setError(error)
        });
    }
  }



  // //for a user who is not a candidate.
  // if (params.id === undefined) {
  //     await axios.post(`https://localhost:7196/api/Candidate`, candidate)
  //         .then(function (response) {
  //             console.log(candidate)
  //             console.log(response);
  //         })
  //         .catch(function (error) {
  //             console.log(candidate)
  //             console.log(error);
  //         });
  // } else {
  //     await axios.put(`https://localhost:7196/api/Candidate/${params.id}`, candidate)
  //         .then(function (response) {
  //             console.log(response);
  //         })
  //         .catch(function (error) {
  //             console.log(error);
  //         });
  // }

  // navigate('/candidate')

  // console.log("THIS IS MINE ", candidate);


  return (
    <div>
      {
        !params.email ?
          <h1 class="display-3 text-center align-middle">Create User</h1> :
          <h1 class="display-3 text-center align-middle">Edit User</h1>
      }
      {error && <ErrorsRegister error={error} />}

      <div>

      </div>
      <Form onSubmit={handleSubmit} className="lead" >
        <Stack gap={3}>
          <Row>
            <Col>
              <Form.Group >
                <Form.Label>Username</Form.Label>
                <Form.Control type="text" name="userName" value={user.userName} onChange={handleChange} required />
              </Form.Group>
            </Col>
            <Col>
              <Form.Group >
                <Form.Label>Email</Form.Label>
                <Form.Control type="text" name="email" value={user.email} onChange={handleChange} />
              </Form.Group>
            </Col>
          </Row>
          <Row>

            <Col>
              <Form.Group >
                <Form.Label>Phone number</Form.Label>
                <Form.Control type="text" name="phoneNumber" value={user.phoneNumber} onChange={handleChange} />
              </Form.Group>
            </Col>
            <Col>
              <Form.Group >
                <Form.Label>Role</Form.Label>
                <Form.Select as="select" name="role"
                  value={user.role}
                  onChange={handleChange}
                  required>
                  <option value="" hidden>Please choose a role... </option>
                  {roles.map((role, index) =>
                    <option key={index}
                      value={role}
                    >{role}</option>
                  )}
                </Form.Select>
              </Form.Group>
            </Col>
          </Row>
          <Row>
            <Row>
              Change Password
            </Row>
            <Col>
              <Form.Group >
                <Form.Label>new Password</Form.Label>
                <Form.Control type="password" name="newPassword1" value={password} onChange={handleChange} />
              </Form.Group>
            </Col>
            <Row>
              <Col>
                <Form.Group >
                  <Form.Label>Confirm new Password</Form.Label>
                  <Form.Control type="password" name="newPassword2" value={confirmPassword} onChange={handleChange} />
                </Form.Group>
              </Col>
            </Row>
          </Row>


          <Button variant="primary" type="submit" >
            Save
          </Button>
        </Stack>
      </Form>
      <Button variant='dark' className='d-grid gap-2 col-12 mx-auto py-2 my-2' onClick={() => navigate(-1)}>Go back</Button>
    </div>
  )
}


export default UserForm;
