import React, { useEffect, useState, useContext } from 'react';
import { AuthenticationContext } from '../auth/AuthenticationContext'
import instance from '../auth/axiosInstance';

import { ListGroup, ListGroupItem, Button, Table, Row, Stack } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import { trackPromise } from 'react-promise-tracker';
import LoadingIndicator from '../Common/LoadingIndicator';


//import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
//import { faEnvelope, faTrashCan } from '@fortawesome/free-solid-svg-icons'


function CertificateList(props) {

    const navigate = useNavigate();
    const [data, setData] = useState([]);
    const { update, claims } = useContext(AuthenticationContext);
    const [role, setRole] = useState(claims.find(claim => claim.name === 'role')?.value)

    //---------------------------------------
    //need to implement random select for examIds
    //-----------------------------------
    //const generateRandomNumber = () => {
    //    const randomNumber = Math.floor(Math.random() * questionAnswer.length);
    //    setRandomNumber(randomNumber)
    //}
    // --------------------------------

    useEffect(() => {
        console.log(data)
        trackPromise(axios.get('https://localhost:7196/api/Certificates').then(res => {
            if (role === "candidate") {
                // if the user is a candidate, show only active products
                console.log(res.data.data.filter(item => item.active === true && item.exams && item.exams.length !== 0))
                setData([...res.data.data.filter(item => item.active === true && item.exams && item.exams.length !== 0)])
                console.log("res.data.data", res.data.data)
            } else {
                //if none of the above, show all products
                setData(res.data.data)
                console.log(res.data.data)
            }
        }).catch(err => {
            console.error(err);
        }));
    }, [])

    const handleBuy = (id) => {
        //console.log("handle buy")
        console.log("data in buy", data)
        const cert = data.filter(item => item.id === id)[0];
        const headers = { "Origin": "https://localhost:44473/" }
        console.log(id)
        axios.get(`https://localhost:7196/api/Paypal/${id}`, { headers }).then(response => {
            console.log(response.data)
            // window.open(response.data, "_blank", "height=600,width=800");
            window.location.href = response.data;
        })
        // console.log("handle buy ",id);
        // console.log("cert in buy",cert)
        // console.log("certexam.id", cert.exams[0].id);
        // axios.post(`https://localhost:7196/api/CandidateExam/CreateCandExam/${id}`)
        //     .then(res => {
        //         console.log(res);
        //     })
        //     .catch(err => {
        //         console.error(err);
        //     });
    }

    const handleDelete = (id) => {
        // Asks user if they are sure
        const confirmDelete = window.confirm("Are you sure you want to delete this certificate?");

        if (confirmDelete) {
            //TODO check for role before delete
            //send axios call with the request to delete using Id
            axios.delete(`https://localhost:7196/api/Certificates/${id}`)
                .then(response => {
                    console.log(response.data.data);
                    let newCerts = data.filter(item => item.id !== id)
                    setData(newCerts)
                    // this.setState(prevState => ({
                    //     data: prevState.data.filter(item => item.id !== id)
                    // }));
                })
                .catch(function(error) {
                    console.log(error);
                });
        }
    };

    const makebuttons = (certId) => {

        if (role === "admin") {
            return (
                <td>
                    <div className='d-flex gap-2'>
                        <Button onClick={() => { navigate(`/certificate/edit/${certId}`) }}>Edit</Button>
                        <Button variant="dark" onClick={() => handleDelete(certId)}>
                            Delete
                        </Button>
                    </div>
                </td>
            );
        } else if ((role === "qualitycontrol")) {
            return (
                <td>
                    <Button onClick={() => { navigate(`/certificate/edit/${certId}`) }}>Details</Button>
                </td>
            );
        } else if ((role === "candidate")) {
            return (
                <td>
                    <div className='d-flex '>
                        <Button variant="outline-success" onClick={() => handleBuy(certId)} >Purchase</Button>
                    </div>
                </td>
            );
        } else {
            return (
                <td>
                    <div className='d-flex '>
                        <Button variant="outline-success" onClick={() => navigate("/register")} >Purchase</Button>
                    </div>
                </td>
            );
        }

    };

    return (
        <div className='container-fluid'>
            <h1 class="display-3 text-center align-middle">Certificates</h1>
            {role === "admin" ?
                <Button variant='dark'
                    className='d-grid gap-2 col-6 mx-auto py-2 my-2'
                    onClick={() => navigate('/certificate/create')}
                > Create a new Certificate </Button>
                : null}
            <Table striped hover borderless className="text-center align-middle" id='list_of_allcerts'>
                <thead>
                    <tr>
                        <th>Title</th>
                        <th>Description</th>
                        <th>Category</th>
                        <th>Price</th>
                        <th className='ml-2' >Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {data.map((certificate, index) => (
                        <tr key={index}>
                            <td>{certificate.title}</td>
                            <td>{certificate.description}</td>
                            <td>{certificate.category}</td>
                            <td>{certificate.price}&euro;</td>
                            {makebuttons(certificate.id)}
                            <td>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </Table>
            <LoadingIndicator />
        </div>
    );
};

export default CertificateList;
