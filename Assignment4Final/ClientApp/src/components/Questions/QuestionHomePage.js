// import {Form,Button,Col,Row,FloatingLabel,Stack,Table,Container,} from "react-bootstrap";

import { Button, Table, Container } from "react-bootstrap";
import React, { useState, useEffect, useContext } from "react";
import axios from "axios";
import { Link } from "react-router-dom";
import { useNavigate } from 'react-router-dom';

import parse from "html-react-parser";
import { AuthenticationContext } from '../auth/AuthenticationContext'
import { trackPromise } from "react-promise-tracker";
import LoadingIndicator from "../Common/LoadingIndicator";

// import QuestionEdit from "./QuestionEdit";

function Questions() {

    const navigate = useNavigate();
    const [data, setData] = useState([]);
    const { update, claims } = useContext(AuthenticationContext);
    const [role, setRole] = useState(claims.find(claim => claim.name === 'role').value)

    useEffect(() => {
        trackPromise(axios
            .get("https://localhost:7196/api/questions")
            .then((response) => {
                console.log(response.data.data)
                setData(response.data.data);
            })
            .catch((error) => {
                console.log(error);
            }));
    }, []);

    //--------------------------------------------------//filters the text from the raw html
    function Replace(temp) {
        return (
            <td>{parse(temp)}</td>
        )
    }
    //--------------------------------------------------HANDLE DELETE
    const handleDelete = (id) => {
        // Asks user if they are sure
        // FIX:(akotro) event.target is undefined
        // const localId = event.target.dataset.id;

        const confirmDelete = window.confirm("Are you sure you want to delete this this question?");

        if (confirmDelete) {
            //send axios call with the request to delete using Id
            axios
                .delete(`https://localhost:7196/api/questions/${id}`)
                .then((response) => {
                    // console.log("Delete response");
                })
                .catch(function(error) {
                    console.log(error);
                });

            const newData = data.filter((item) => { return item.id != id; }, id);
            setData(newData);
        }
    }

    const makebuttons = (queId) => {
        if (role === "qualitycontrol") {
            return (
                <td>
                    <div className='d-flex '>
                        <Button onClick={() => { navigate(`/questions/edit/${queId}`) }}>Details</Button>
                    </div>
                </td>
            );
        } else if ((role === "admin")) {
            return (
                <td>
                    <div className='d-flex gap-2'>
                        <Button onClick={() => { navigate(`/questions/edit/${queId}`) }}>Edit</Button>
                        <Button variant="dark" onClick={() => handleDelete(queId)}>Delete</Button>
                    </div>
                </td>
            );
        }
        return (
            <td>

            </td>
        );

    };

    //     <td>
    //     <Link to={`/questions/edit/${item.id}`} state={{ questionIndex: item.id }}>
    //         <Button >Edit</Button>
    //     </Link>
    // </td>

    //  <td>
    //                                     <Button
    //                                         variant="dark"
    //                                         onClick={handleDelete}
    //                                         data-id={item.id}>
    //                                         Delete
    //                                     </Button>
    //                                 </td>

    //-----------------------------------------------------------
    return (
        <Container fluid="md">
            <h1 class="display-3 text-center align-middle">Questions</h1>
            {role === "admin" ?
                <Button variant='dark'
                    className='d-grid gap-2 col-6 mx-auto py-2 my-2'
                    onClick={() => navigate('/questions/create')}
                > Create a new Question </Button>
                : null}
            <div>
                <Table hover striped>
                    <thead>
                        <tr>
                            <th scope="Col">Id</th>
                            <th scope="col">MainText</th>
                            <th scope="col">Topic</th>
                            <th scope="col">Actions</th>
                        </tr>
                    </thead>

                    <tbody>
                        {data.map((item, index) => (
                            <tr key={item.id}>
                                <td>{item.id != null && item.id}</td>
                                {Replace(item.text)}
                                <td>
                                    {item.topic === undefined || item.topic === null
                                        ? "No topic selected"
                                        : item.topic.name}
                                </td>
                                {makebuttons(item.id)}
                            </tr>
                        ))}
                    </tbody>
                </Table>
                <LoadingIndicator />
            </div>
        </Container>
    );
}

export default Questions;
