import React, { useEffect, useState, useContext } from "react";
import CandidateEdit from "../Candidate_CRUD/CandidateEdit";
import { useNavigate, Link, useParams } from "react-router-dom";
import { AuthenticationContext } from '../auth/AuthenticationContext'

import { ListGroup, ListGroupItem, Button, Table, Row, Col, Stack, Form, CloseButton, FormGroup, InputGroup } from 'react-bootstrap';

import axios from 'axios';
import { useLocation } from 'react-router-dom'

function ExamQuestionList() {

    const params = useParams();
    const location = useLocation();
    let navigate = useNavigate();
    // const Data = location.state.data;

    const [questions, setQuestions] = useState([]);
    const [exam, setExam] = useState();
    const [mark, setMark] = useState(0)
    const { update, claims } = useContext(AuthenticationContext);
    const role = claims.find(claim => claim.name === 'role').value


    const [previousLocation, setPreviousLocation] = useState(location);

    useEffect(() => {
        axios.get(`https://localhost:7196/api/Exam/${params.id}`).then((response) => {
            console.log('useeffect resp', response)
            setExam(response.data.data)
            setQuestions(response.data.data.questions)
        })
            .catch(function (error) {
                console.log(error);
            });
    }, []);


    const handleAdd = (exam) => {
        axios.put(`https://localhost:7196/api/Exam/${exam.id}`, exam).then(setExam(exam))
        navigate(`/AddQuestionToExam/${exam.id}`)
    }

    const handleRemove = (questR) => {
        const confirmDelete = window.confirm("Are you sure you want to delete this question?");
        exam.questions = exam.questions.filter(q => q.id !== questR.id)
        if (confirmDelete) {
            axios.put(`https://localhost:7196/api/Exam/${exam.id}`, exam).then(setExam(exam))
            setQuestions(exam.questions)

        }
        console.log(questions)
        var kati = exam.questions;
    }

    const makeButtons = (exam, question) => {

        return (

            <div>
                <Button onClick={() => handleRemove(question)} variant="outline-danger">Remove</Button>

            </div>
        )
    }



    function Replace(temp) {
        var parser = new DOMParser();

        var doc = parser.parseFromString(temp, 'text/html');

        return doc.body.innerText;

    }
    const handleChangePassmark = (event) => {
        console.log('EXAAAAMMM', exam)
        console.log(event.target.value)
        setMark(event.target.value)
        setExam({ ...exam, passMark: Number(event.target.value) })
        console.log('examafterrr', exam)
    }

    const handleBack = () => {
        console.log("before back", exam)
        axios.put(`https://localhost:7196/api/Exam/${exam.id}`, exam).then(navigate("/ExamsList/"))
            .catch(function (error) {
            });
    }

    return (
        <div>


            <fieldset disabled={role ? role === "qualitycontrol" : false}>
                <div className="my-2 ">

                    <Row className="mt-3">
                        <Col xs={3}>
                            <span class="p-3 mb-2 rounded border border-dark text-dark rounded">
                                Questions in Exam: {questions.length}
                            </span>
                        </Col>
                        <Col xs={3}>
                            <span class="p-3 mb-2 rounded border border-dark text-dark rounded">
                                EASY: {questions.filter(quest => quest.difficultyLevel.difficulty !== "HARD" && quest.difficultyLevel.difficulty !== "MEDIUM").length}
                            </span>
                        </Col>
                        <Col xs={3}>
                            <span class="p-3 mb-2 rounded border border-dark text-dark rounded">
                                MEDIUM: {questions.filter(quest => quest.difficultyLevel.difficulty !== "EASY" && quest.difficultyLevel.difficulty !== "HARD").length}
                            </span>
                        </Col>
                        <Col xs={3}>
                            <span class="p-3 mb-2 rounded border border-dark text-dark rounded">
                                HARD: {questions.filter(quest => quest.difficultyLevel.difficulty !== "EASY" && quest.difficultyLevel.difficulty !== "MEDIUM").length}
                            </span>
                        </Col>
                    </Row>
                    <Row>
                        <p></p>
                        <Button onClick={() => handleAdd(exam)} variant='dark'
                            className='d-grid gap-2 col-6 mx-auto py-2 my-2' >Add Question</Button>
                    </Row>
                    {exam && (
                        <Row className="mt-3">
                            <Col xs={3}>
                                <InputGroup>
                                    <InputGroup.Text>Set Passing Mark</InputGroup.Text>
                                    <Form.Control
                                        placeholder="Set Passing Mark"
                                        type="number"
                                        value={exam.passMark}
                                        onChange={handleChangePassmark}
                                    />
                                </InputGroup>
                            </Col>
                        </Row>
                    )}
                </div>
                <table className="mt-3">
                    <thead>
                        <tr>
                            <th>Question Text</th>
                            <th>Difficulty Level</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {questions.map((quest, index) => (
                            <tr key={index}>
                                <td>{Replace(quest.text)}</td>
                                <td>{quest.difficultyLevel.difficulty}</td>
                                <td>{makeButtons(exam, quest)}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </fieldset>
            <Button variant='secondary' className='d-grid gap-2 col-12 mx-auto py-2 my-2' onClick={handleBack} >Go Back To Exams List</Button>
        </div>
    )
}
export default ExamQuestionList;
