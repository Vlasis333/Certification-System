import React, { useEffect, useState } from "react";
import { ListGroup, ListGroupItem, Button, Table, Row, Stack, Form, Modal } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { withRouter } from './../Common/with-router';
import { BrowserRouter, Route, useParams } from "react-router-dom";
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import { DatePicker, Space } from 'antd';
import { trackPromise } from "react-promise-tracker";
import LoadingIndicator from "../Common/LoadingIndicator";

function AvailableExams(props) {

    const [data, setData] = useState([]);
    const [exams, setExams] = useState([]);
    const [takenExams, setTakenExams] = useState([]);
    const [user, setUser] = useState();
    const [bookExam, setBookExam] = useState();
    let navigate = useNavigate();
    const candidate = {};

    useEffect(() => {
        trackPromise(axios.get('https://localhost:7196/api/CandidateExam').then((response) => {
            setData(response.data);
            console.log(response.data);

            setExams([...response.data.filter(exam => exam.result === undefined)])
            setTakenExams([...response.data.filter(exam => (exam.result === true || exam.result === false))])
            // console.log(response.data);
            console.log(response.data[0].candidate);
            if (response.data.length > 0) {
                candidate = response.data[0].candidate
            }
            // console.log(...response.data.filter(exam => exam.result !== null));
        }).catch(function(error) {
            console.log(error);
        }));
    }, []);

    const [showModal, setShowModal] = useState(false);
    const [selectedDate, setSelectedDate] = useState(null);

    const saveDate = () => {
        setShowModal(false);
        // exams.map(exam=>exam.id === bookExam.id)
        setExams(exams.map(exam => {
            if (exam.id === bookExam.id) {
                return bookExam
            }
            return exam;
        }));
    }

    const convertStringToDate = (dateString) => {
        //intial format
        //2015-07-15
        const date = new Date(dateString);
        //Wed Jul 15 2015 00:00:00 GMT-0700 (Pacific Daylight Time)
        const finalDateString = date.toISOString(date);
        //2015-07-15T00:00:00.000Z

        // console.log(finalDateString); // "1930-07-17T00:00:00.000Z"
        return finalDateString;
    };

    const handleChange = (event) => {
        // console.log("handlechnage");
        const { name, value, type } = event.target;
        // console.log(name);
        //console.log(type);
        // console.log(value);
        setSelectedDate(value);
        setBookExam({ ...bookExam, [name]: convertStringToDate(value) })
    }
    const handleOpen = (CandExamId) => {
        setShowModal(true);
        setSelectedDate(new Date());
        setBookExam({ ...exams.find(exam => exam.id === CandExamId) })
        // console.log(CandExamId);
    };

    const handleClose = () => {
        setShowModal(false);
    };

    const makebuttons = (CandExam) => {
        if ((CandExam.result === true || CandExam.result === false)) {
            return (
                <td>
                    <div className='d-grid justify-content-md-end '>
                        <Button variant="success" onClick={() => showResultsOfExam(CandExam)}>View Result</Button>
                    </div>
                </td>
            );
        } else {
            return (
                <td>
                    <div className='d-grid gap-2 d-md-flex justify-content-md-end'>
                        <Button onClick={() => handleOpen(CandExam.id)}>Book Date</Button>
                        <Button variant="success" onClick={() => takeExam(CandExam)}>Take Exam!</Button>
                    </div>
                </td>
            );
        }
    };

    const showResultsOfExam = (CandExam) => {
        // console.log(CandExam)
        navigate(`/candidate/ExamResults`, {
            state: {
                data: CandExam,
                from: '/candidate/availableexams',
                candidate: candidate
            }
        });
    }

    const takeExam = (CandExam) => {
        const currentDate = new Date();
        const examDate = new Date(CandExam.examDate);

        if (examDate.getTime() < currentDate.getTime()) {
            alert("The exam date has passed.");
            return;
        }

        navigate(`/candidate/Examination/${CandExam.id}`, { state: { candidate: candidate } });
    };

    const makeDate = (examDate) => {
        if (!examDate) {
            return "";
        }

        let date = new Date(examDate);
        let options = { year: 'numeric', month: 'long', day: 'numeric', hour: 'numeric', minute: 'numeric' };

        if (date && date.toString() !== "Invalid Date") {
            return date.toLocaleDateString('en-US', options);
        } else {
            return "";
        }
    }

    return (
        <div className='container-fluid'>
            <h1 class="display-3 text-center align-middle">Examinations</h1>
            <div>
                <Modal show={showModal} onHide={handleClose}>
                    <Modal.Header closeButton>
                        <Modal.Title>Choose a date for your Exam</Modal.Title>
                    </Modal.Header>
                    <Modal.Body><Form.Group >
                        <Form.Label>Date of Birth</Form.Label>
                        <Form.Control type="date"
                            name="examDate"
                            value={selectedDate}
                            onChange={handleChange} />
                    </Form.Group></Modal.Body>
                    <Modal.Footer>
                        <Button variant="secondary" onClick={handleClose}>Close</Button>
                        <Button variant="primary" onClick={() => saveDate()}>Save Changes</Button>
                    </Modal.Footer>
                </Modal>

                <h1>Available exams</h1>
                <Table striped borderless hover>
                    <thead>
                        <tr>
                            <th style={{ width: "40%" }}>Title</th>
                            <th style={{ width: "20%" }}>Voucher</th>
                            <th style={{ width: "20%" }}>Exam Date</th>
                            <th ></th>
                        </tr>
                    </thead>
                    <tbody>
                        {exams.map((CandidateExam, index) => {
                            if (CandidateExam !== undefined && CandidateExam.exam !== undefined && CandidateExam.exam.certifiate !== null) {
                                return (
                                    <tr key={index}>
                                        <td>{CandidateExam.exam.certificate.title}</td>
                                        <td>{CandidateExam.voucher}</td>
                                        <td>{makeDate(CandidateExam.examDate)}</td>
                                        <td>
                                            {makebuttons(CandidateExam)}
                                        </td>
                                    </tr>
                                )
                            }
                        })}
                    </tbody>
                </Table>
                <LoadingIndicator />
            </div>
            <div>
                <h1>Taken exams</h1>
                <Table striped borderless hover>
                    <thead>
                        <tr>
                            <th style={{ width: "40%" }}>Title</th>
                            <th style={{ width: "20%" }}>Exam Date</th>
                            <th style={{ width: "20%" }}>Score</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        {takenExams.map((CandidateExam, index) => {
                            if (CandidateExam !== undefined && CandidateExam.exam !== undefined && CandidateExam.exam.certificate !== undefined) {
                                return (
                                    <tr key={index}>
                                        <td>{CandidateExam.exam.certificate.title}</td>
                                        <td>{makeDate(CandidateExam.examDate)}</td>
                                        <td>{CandidateExam.percentScore}&nbsp;%</td>
                                        <td>
                                            {makebuttons(CandidateExam)}
                                        </td>
                                    </tr>

                                )
                            }
                        })}
                    </tbody>
                </Table>
                <LoadingIndicator />
            </div>
        </div>
    );
};

export default AvailableExams;
