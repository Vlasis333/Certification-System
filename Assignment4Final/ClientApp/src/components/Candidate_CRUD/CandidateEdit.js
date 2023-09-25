import React, { useEffect, useState, useContext } from "react";
import { ListGroup, ListGroupItem, Button, Table, Row, Col, Stack, Form, CloseButton, Alert } from 'react-bootstrap';
import { AuthenticationContext } from '../auth/AuthenticationContext'
import { getUserId } from '../auth/handleJWT'
import { useNavigate, useParams, useLocation } from "react-router-dom";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import axios from "axios";
import Errors from '../Common/ErrorList';
import ErrorsRegister from '../Common/ErrorListRegister'


export default function CandidateEdit(props) {

    const params = useParams();
    const navigate = useNavigate();
    const location = useLocation();

    const Qcrole = location.state?.role;

    const [genders, setGenders] = useState([]);
    const [languages, setLanguages] = useState([]);
    const [photoIdTypes, setPhotoIdTypes] = useState([]);
    const [countries, setCountries] = useState([]);
    const [allUsers, setAllusers] = useState([]);
    const [message, setMesage] = useState();
    const [registerButton, setRegisterButton] = useState(false)
    const [error, setError] = useState(null);
    const [errorRegister, setErrorRegister] = useState(null);

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

    const { update, claims } = useContext(AuthenticationContext);
    const [role, setRole] = useState({})

    const [registerCand, setRegisterCand] = useState({});

    const [candidate, setCandidate] = useState({
        dateOfBirth: null,
        photoIdIssueDate: null,
        gender: [],
        language: [],
        photoIdType: [],
        address: []
    });

    const fetchData = () => {
        if (params.id !== undefined) {
            axios
                .get(`https://localhost:7196/api/Candidate/${params.id}`)
                .then((response) => {
                    setCandidate(response.data.data);
                })
                .catch(function (error) {
                    console.log(error);
                });
        } else {
            axios
                .get(`https://localhost:7196/api/accounts/listUsers`)
                .then((response) => {
                    setAllusers(response.data);
                })
                .catch(function (error) {
                    console.log(error);
                });
        }

        axios
            .get(`https://localhost:7196/api/Genders`)
            .then((response) => {
                setGenders(response.data.data);
            })
            .catch(function (error) {
                console.log(error);
            });

        axios
            .get(`https://localhost:7196/api/Languages`)
            .then((response) => {
                setLanguages(response.data.data);
            })
            .catch(function (error) {
                console.log(error);
            });

        axios
            .get(`https://localhost:7196/api/PhotoIdTypes`)
            .then((response) => {
                setPhotoIdTypes(response.data.data);
            })
            .catch(function (error) {
                console.log(error);
            });

        axios
            .get(`https://localhost:7196/api/Countries`)
            .then((response) => {
                setCountries(response.data.data);
            })
            .catch(function (error) {
                console.log(error);
            });
    };

    const handleChangeRegister = (event) => {
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
        }
        else {
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

    const getId = () => {
        if (claims.length > 0) {
            return claims.find(claim => claim.name === 'userId').value
        }
    }

    useEffect(() => {
        fetchData();
        if (claims.length > 0) {
            // is a user with a role already then set that role 
            if (claims.find(claim => claim.name === 'role')) {
                setRole(claims.find(claim => claim.name === 'role').value)
                // else set "NoRole" as the role 
            } else {
                setRole("noRole");
            };
            // console.log(role)
        }
    }, [claims]);

    // const handleRegisterSubmit = (event) => {
    //     event.preventDefault();

    //     axios.post(`https://localhost:7196/api/accounts/create`, registerCand).then(res => {
    //         const Id = getUserId(res.data.token)
    //         setCandidate((prev) => ({ ...prev, appUserId: Id }))
    //     }).catch(function (error) {
    //     });
    // }

    const handleSubmitRegister = (event) => {
        event.preventDefault();

        axios.post(`https://localhost:7196/api/accounts/create`, user)
            .then(response => {
                console.log(response);
                alert("Register successful !! ")
                setRegisterButton(true)
                setErrorRegister(null);
            })
            .catch(err => {
                console.error(err);
                setErrorRegister(err);
            });
    };

    const handleSubmit = async (event) => {
        event.preventDefault();
        //for a user who is not a candidate.
        if (params.id === undefined) {
            try {
                const response = await axios.post(`https://localhost:7196/api/Candidate`, candidate);
                console.log(response);
                setError([]);
                navigate('/');
            } catch (e) {
                setError(e);
            }
        } else {
            try {
                const response = await axios.put(`https://localhost:7196/api/Candidate/${params.id}`, candidate);
                console.log(response);
                setError([]);
                navigate('/candidate');
            } catch (e) {
                setError(e);
            }
        }
    }

    const handleChange = (event, addressIndex) => {
        if (!candidate.appUserId) {
            let id = getId();
            candidate.appUserId = id;
            setCandidate({ candidate })
        }
        const { name, value, type } = event.target;
        //console.log(event.target)
        //console.log(name);
        //console.log(type);
        //console.log(value);
        if (addressIndex === undefined) {
            if (name === "gender") {
                setCandidate({ ...candidate, [name]: genders.find(item => item.id === Number(value)) });
            } else if (name === "language") {
                setCandidate({ ...candidate, [name]: languages.find(item => item.id === Number(value)) });
            } else if (name === "photoIdType") {
                setCandidate({ ...candidate, [name]: photoIdTypes.find(item => item.id === Number(value)) });
            } else if (type === "date") {
                setCandidate({ ...candidate, [name]: convertStringToDate(value) });
            } else {
                setCandidate({ ...candidate, [name]: value });
            }
        } else {
            setCandidate({
                ...candidate, address: candidate.address.map((address, index) => {
                    if (index === addressIndex) {
                        if (name === "country") {
                            return {
                                ...address, [name]: countries.find(item => item.id === Number(value))
                            };
                        } else {
                            return {
                                ...address, [name]: value
                            };
                        }
                    }
                    return address;
                })
            });
        }
    };

    const addAddress = () => {
        setCandidate({
            ...candidate,
            address: [
                ...candidate.address,
                { id: 0, address1: "", address2: "", city: "", state: "", country: {} },
            ],
        });
    };

    const removeAddress = (removeIndex) => {
        const updatedAddress = [...candidate.address];
        updatedAddress.splice(removeIndex, 1);
        console.log(updatedAddress);
        setCandidate({ ...candidate, address: updatedAddress });
    };
    const convertStringToDate = (dateString) => {
        //intial format
        //2015-07-15
        const date = new Date(dateString);
        //Wed Jul 15 2015 00:00:00 GMT-0700 (Pacific Daylight Time)
        const finalDateString = date.toISOString(date);
        //2015-07-15T00:00:00.000Z

        console.log(finalDateString); // "1930-07-17T00:00:00.000Z"
        return finalDateString;
    };

    const convertDateToString = (date) => {
        //console.log(date);
        let kati = new Date(date);

        let formattedDate = kati.toISOString().substr(0, 10);

        return formattedDate;
    };

    return (
        <div>
            {
                params.id === undefined ?
                    <h1 class="display-3 text-center align-middle">Create Candidate</h1> :
                    role && role !== "qualitycontrol" ?
                        <h1 class="display-3 text-center align-middle">Edit Candidate</h1> :
                        <h1 class="display-3 text-center align-middle">Candidate Details</h1>

            }
            {error && <Errors error={error} />}
            <fieldset disabled={Qcrole ? (Qcrole === "qualitycontrol") : false}>
                {errorRegister && <ErrorsRegister error={errorRegister} />}
                {!params.id &&
                    (role === "admin") ?
                    <div>

                        <Form onSubmit={handleSubmitRegister}>
                            <Form.Group controlId="formBasicEmail">
                                <Form.Label>Email address</Form.Label>
                                <Form.Control
                                    type="email"
                                    placeholder="Enter email"
                                    name="Email"
                                    value={user.Email}
                                    onChange={handleChangeRegister}
                                />
                            </Form.Group>
                            <Form.Group controlId="formBasicUserName">
                                <Form.Label>Username</Form.Label>
                                <Form.Control
                                    type="text"
                                    placeholder="Enter username"
                                    name="UserName"
                                    value={user.UserName}
                                    onChange={handleChangeRegister}
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
                                <Button className='d-grid gap-2 col-6 mx-auto py-2 my-2' variant="primary" type="submit" disabled={registerButton}>
                                    Register User
                                </Button>
                            </Form.Group>
                        </Form>
                    </div> : null}
                <Form onSubmit={handleSubmit} className="lead" >
                    <Stack gap={3}>
                        <div className="display-6 fs-2" >Personal Details</div>
                        <Row>
                            <Col>
                                <Form.Group >
                                    <Form.Label>First Name</Form.Label>
                                    <Form.Control type="text" name="firstName" value={candidate.firstName} onChange={handleChange} required />
                                </Form.Group>
                            </Col>
                            <Col>
                                <Form.Group >
                                    <Form.Label>Middle Name</Form.Label>
                                    <Form.Control type="text" name="middleName" value={candidate.middleName} onChange={handleChange} />
                                </Form.Group>
                            </Col>
                            <Col>
                                <Form.Group >
                                    <Form.Label>Last Name</Form.Label>
                                    <Form.Control type="text" name="lastName" value={candidate.lastName} onChange={handleChange} required />
                                </Form.Group>
                            </Col>
                            <Col>
                                <Form.Group >
                                    <Form.Label>Gender</Form.Label>
                                    <Form.Select as="select" name="gender"
                                        value={candidate.gender.id}
                                        onChange={handleChange}
                                        required>
                                        <option value="" hidden>Please choose a gender... </option>
                                        {genders.map((gender, index) =>
                                            <option key={index}
                                                value={gender.id}
                                            >{gender.genderType}</option>
                                        )}
                                    </Form.Select>
                                </Form.Group>
                            </Col>
                        </Row>
                        <hr />
                        <Row>
                            <Col>
                                <Form.Group >
                                    <Form.Label>Date of Birth</Form.Label>
                                    <Form.Control type="date"
                                        name="dateOfBirth"
                                        value={convertDateToString(candidate.dateOfBirth)}
                                        onChange={handleChange} />
                                </Form.Group>
                            </Col>
                            <Col>
                                <Form.Group >
                                    <Form.Label>Native Language</Form.Label>
                                    <Form.Select as="select" name="language"
                                        value={candidate.language.id}
                                        onChange={handleChange}>
                                        <option value="" hidden >Please choose your native Language... </option>
                                        {languages.map((lan, index) =>
                                            <option key={index}
                                                value={lan.id}
                                            >{lan.nativeLanguage}</option>
                                        )}
                                    </Form.Select>
                                </Form.Group>
                            </Col>
                        </Row>
                        <hr />
                        <div className="display-6 fs-2" >Contact Details</div>
                        <Row>
                            <Col>
                                <Form.Group >
                                    <Form.Label>Email</Form.Label>
                                    <Form.Control type="email" name="email" value={candidate.email} onChange={handleChange} required />
                                </Form.Group>
                            </Col>
                            {
                                (role === "candidate") ?
                                <></>
                                :
                                <Col>
                                    <Form.Group >
                                        <Form.Label>Candidate Number</Form.Label>
                                        <Form.Control type="number" name="candidateNumber" value={candidate.candidateNumber} onChange={handleChange} />
                                    </Form.Group>
                                </Col> }
                            <Col>
                                <Form.Group >
                                    <Form.Label>Landline Number</Form.Label>
                                    <Form.Control type="text" name="landline" value={candidate.landline} onChange={handleChange} />
                                </Form.Group>
                            </Col>
                            <Col>
                                <Form.Group >
                                    <Form.Label>Mobile Number</Form.Label>
                                    <Form.Control type="text" name="mobile" value={candidate.mobile} onChange={handleChange} required />
                                </Form.Group>
                            </Col>
                        </Row>
                        <div>
                            <Stack gap={3}>
                                {candidate.address &&
                                    candidate.address.map((item, index) => (
                                        <div key={index} name={item.id} className="my-1 ">
                                            <Row>
                                                <details className="display-6 fs-4">
                                                    <summary>
                                                        Address {index + 1}
                                                    </summary>
                                                    <div className="card card-body ">
                                                        <div className="justify-content-end">
                                                            <CloseButton onClick={() => removeAddress(index)} />
                                                        </div>
                                                        <Row>
                                                            <Col>
                                                                <Form.Group >
                                                                    <Form.Label>address1</Form.Label>
                                                                    <Form.Control type="text" name="address1" value={item.address1} onChange={(event) => handleChange(event, index)} />
                                                                </Form.Group>
                                                            </Col>
                                                            <Col>
                                                                <Form.Group >
                                                                    <Form.Label>address2</Form.Label>
                                                                    <Form.Control type="text" name="address2" value={item.address2} onChange={(event) => handleChange(event, index)} />
                                                                </Form.Group>
                                                            </Col>
                                                            <Col>
                                                                <Form.Group >
                                                                    <Form.Label>postalCode</Form.Label>
                                                                    <Form.Control type="text" name="postalCode" value={item.postalCode} onChange={(event) => handleChange(event, index)} />
                                                                </Form.Group>
                                                            </Col>
                                                        </Row>
                                                        <Row>
                                                            <Col>
                                                                <Form.Group >
                                                                    <Form.Label>city</Form.Label>
                                                                    <Form.Control type="text" name="city" value={item.city} onChange={(event) => handleChange(event, index)} />
                                                                </Form.Group>
                                                            </Col>
                                                            <Col>
                                                                <Form.Group >
                                                                    <Form.Label>state</Form.Label>
                                                                    <Form.Control type="text" name="state" value={item.state} onChange={(event) => handleChange(event, index)} />
                                                                </Form.Group>
                                                            </Col>
                                                            <Col>
                                                                <Form.Group >
                                                                    <Form.Label>country</Form.Label>
                                                                    <Form.Select as="select" name="country"
                                                                        value={item.country.id}
                                                                        onChange={(event) => handleChange(event, index)} required>
                                                                        <option value="" hidden >Please choose your Country... </option>
                                                                        {countries.map((country, index) =>
                                                                            <option key={index}
                                                                                value={country.id}
                                                                            >{country.countryOfResidence}</option>
                                                                        )}
                                                                    </Form.Select>
                                                                </Form.Group>
                                                            </Col>
                                                        </Row>
                                                    </div>
                                                </details>
                                            </Row>
                                        </div>
                                    ))}
                                {role.value !== "qualitycontrol" &&
                                    <Button onClick={addAddress} className='d-grid gap-2 col-6 mx-auto py-2 my-2' >add address</Button>
                                }
                            </Stack>
                        </div>
                        <hr />
                        <div className="display-6 fs-2" >Identification Details</div>
                        <Row>
                            <Col>
                                <Form.Group >
                                    <Form.Label>Photo ID Number</Form.Label>
                                    <Form.Control type="text" name="photoIdNumber" value={candidate.photoIdNumber} onChange={handleChange} required />
                                </Form.Group>
                            </Col>
                            <Col>
                                <Form.Group >
                                    <Form.Label>Photo Issue Date</Form.Label>
                                    <Form.Control type="date"
                                        name="photoIdIssueDate"
                                        value={convertDateToString(candidate.photoIdIssueDate)}
                                        onChange={handleChange} required />
                                </Form.Group>
                            </Col>
                            <Col>
                                <Form.Group >
                                    <Form.Label>Type of Id</Form.Label>
                                    <Form.Select as="select" name="photoIdType"
                                        value={candidate.photoIdType.id}
                                        onChange={handleChange}
                                        required>
                                        <option value="" hidden >Please choose your ID type... </option>
                                        {photoIdTypes.map((pId, index) =>
                                            <option key={index}
                                                value={pId.id}
                                            >{pId.idType}</option>
                                        )}
                                    </Form.Select>
                                </Form.Group>
                            </Col>
                        </Row>
                        {role.value === "qualitycontrol" ? null :
                            <Button variant="primary" type="submit" >
                                Save
                            </Button>}
                    </Stack>
                </Form>
            </fieldset>
            <Button variant='dark' className='d-grid gap-2 col-12 mx-auto py-2 my-2' onClick={() => navigate(-1)}>Go back</Button>
        </div>
    )
}
