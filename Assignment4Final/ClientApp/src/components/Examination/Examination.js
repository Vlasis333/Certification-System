import React, { useEffect, useState } from "react";
import { useNavigate, useParams, useLocation } from "react-router-dom";
import { Button, Col, Row, Form } from 'react-bootstrap';

import axios from 'axios';
import parse from "html-react-parser";
export default function Examination(props) {

    let navigate = useNavigate();
    let location = useLocation();
    const candidate = {};
    const params = useParams();
    const [candidateExam, setCandidateExam] = useState({ exam: { questions: [] } })
    const [user, setUser] = useState();
    const candExamId = params.id;

    // const [candidateExam, setCandidateExam] = useState({
    //     exam: { questions: [] },
    //     CandidateExamAnswers: { ChoosenOptions: [] },
    // });

    useEffect(() => {
        axios.put(`https://localhost:7196/api/CandidateExam/StartExam/${candExamId}`).then((response) => {
            setCandidateExam(response.data);
            console.log(response.data);
            // edw prpei na mpei o candidate kai meta sto apo katw assign 
            // candidate = location.state.candidate;

        }).catch(function (error) {
            console.log(error);
        });
    }, []);

    // function Replace(temp) {
    //     var parser = new DOMParser();

    //     var doc = parser.parseFromString(temp, 'text/html');

    //     return doc.body.innerText;
    // }
    function Replace(temp) {
        return (
            <td>{parse(temp)}</td>
        )
    }

    const saveCandidateExam = () => {

        const confirmDelete = window.confirm("Are you sure you want to submit this?\nThis will finilise all the progress until now !\nWe suggest your try answering EVERY question");

        if (confirmDelete) {
        axios.put(`https://localhost:7196/api/CandidateExam/EndExam/${candExamId}`).then((response) => {
            //console.log(response.data);
            navigate(`/candidate/ExamResults`, { state: { data: response.data, candidate: candidate } });
        })
            .catch(error => {
                console.error(error);
            });
        }
    }

    const handleOptionChange = (event, questionIndex) => {
        // Note(vmavraganis): Code used to keep the selected option of the user for each question (and update if its the correct or not)
        const chosenOption = event.target.value;
        // console.log(questionIndex);
        //console.log(candidateExam.candidateExamAnswers);
        var examAnswersIndex = 0;
        const updatedCandidateExamAnswers = candidateExam.candidateExamAnswers.map((answer, index) => {
            if (index === questionIndex) {
                examAnswersIndex = index;
                return { ...answer, chosenOption };
            }
            return answer;
        });
        setCandidateExam({ ...candidateExam, candidateExamAnswers: updatedCandidateExamAnswers });

        updateCandidateExamAnswer(updatedCandidateExamAnswers[examAnswersIndex].id, updatedCandidateExamAnswers[examAnswersIndex]);
    };

    const updateCandidateExamAnswer = (id, candidateExamAnswerDto) => {
        console.log(candidateExamAnswerDto);
        axios.put(`https://localhost:7196/api/CandidateExamAnswers/${id}`, candidateExamAnswerDto)
            .catch(error => {
                console.error(error);
            });
    }

    // Note(vmavraganis): Code used to slice the array of questions and make them 1 per page (with each question having an array of their options)
    const [currentPage, setCurrentPage] = useState(1);
    const questionsPerPage = 1;
    const questions = candidateExam?.exam?.questions;
    const totalPages = questions ? Math.ceil(questions.length / questionsPerPage) : 0;

    const indexOfLastQuestion = currentPage * questionsPerPage;
    const indexOfFirstQuestion = indexOfLastQuestion - questionsPerPage;
    const currentQuestions = questions ? questions.slice(indexOfFirstQuestion, indexOfLastQuestion) : [];

    return (
        <div>
            <h1 class="display-3 text-center align-middle">Examination</h1>
            <h1>Question {currentPage}</h1>
            <hr></hr>

            {currentQuestions.map((question, index) => (
                <div key={index}>

                    <h3>{ Replace(question.text) }</h3>

                    <ol>
                        {question.options.map((option, index) => (
                            <li key={option.text}>{Replace(option.text)}</li>
                        ))}
                    </ol>

                    {/* <select
                        id="option"
                        value={candidateExam?.candidateExamAnswers[indexOfFirstQuestion + index]?.chosenOption || ""}
                        onChange={(event) => handleOptionChange(event, indexOfFirstQuestion + index)}
                    >
                        <option value="" disabled>Choose option</option>
                        {question.options.map((option, index) => (
                            <option key={index} value={option.text}>
                                {index + 1}
                            </option>
                        ))}
                    </select> */}
                    <Row>

                    <Col xs={4}>

                        <Form.Select as="select" name="gender" id="option"
                            value={candidateExam?.candidateExamAnswers[indexOfFirstQuestion + index]?.chosenOption || ""}
                            onChange={(event) => handleOptionChange(event, indexOfFirstQuestion + index)}
                            required>
                            <option value="" hidden>Choose option</option>
                            {question.options.map((option, index) => (
                                <option key={index} value={option.text}>
                                    {index + 1}
                                </option>)
                            )}
                        </Form.Select>
                    </Col>
                    <Col >
                        <div className="d-grid gap-2 d-md-flex justify-content-md-end">
                            {currentPage !== 1 && <Button onClick={() => setCurrentPage(currentPage - 1)}>Previous</Button>}
                            &nbsp;
                            {currentPage < totalPages && <Button onClick={() => setCurrentPage(currentPage + 1)}>Next</Button>}
                        </div>
                    </Col>
                    </Row>

                </div>
            ))}


            <hr></hr>
            <div className="d-grid gap-2 col-3 mx-auto py-2 my-2">
            <Button  variant="outline-success" onClick={saveCandidateExam}>End & Submit Exam</Button>

            </div>


        </div>
    );
};

