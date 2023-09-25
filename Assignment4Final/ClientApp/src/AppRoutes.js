// import ApiAuthorzationRoutes from "./components/api-authorization/ApiAuthorizationRoutes";
import Home from "./components/Home";

import CertificateForm from './components/Certificate_CRUD/CertificateForm'
import CertificateList from './components/Certificate_CRUD/CertificateList'
import NotFound from './components/Common/NotFound';
import NotAuth from './components/auth/NotAuth';

import Questions from "./components/Questions/QuestionHomePage";
import QuestionEdit from "./components/Questions/QuestionEdit";
import QuestionCreate from "./components/Questions/QuestionCreate";

import CandidateList from "./components/Candidate_CRUD/CandidateList";
import CandidateEdit from "./components/Candidate_CRUD/CandidateEdit";

// import CandidateHomepage from "./components/Candidate_Page/CandidateHomepage";
import AvailableExams from "./components/Candidate_Page/AvailableExams";
import Examination from "./components/Examination/Examination";
import ExamList from "./components/Exam_CRUD/ExamList";
import ExamQuestionList from "./components/Exam_CRUD/ExamQuestionList";

import Login from './components/auth/Login'
import AddQuestionToExam from './components/Exam_CRUD/AddQuestionToExam';

import MarkerList from "./components/Marker/MarkerList";
import MarkExam from "./components/Marker/MarkerMarkExam";
import AssignToMarker from "./components/Admin/AssingToMarker";
import ExamResults from "./components/Examination/Results";
import UsersList from "./components/Admin/UsersList";
import UserForm from "./components/Admin/UserForm";
import RegisterForm from "./components/auth/RegisterForm";

const AppRoutes = [
  {
    index: true,
    element: <Home />,
  },
  {
    path: "/login",
    element: <Login />,
  },
  {
    path: "/register",
    element: <RegisterForm />,
  },
  //-----------------Certificates-------------------
  {
    path: "/certificate",
    needsAdmin: true,
    needsQc: true,
    needsCand: true,
    element: <CertificateList />,
  },
  {
    path: "/certificate/create",
    needsAdmin: true,
    element: <CertificateForm />,
  },
  {
    path: "/certificate/edit/:id",
    needsAdmin: true,
    element: <CertificateForm />,
  },
  //-----------------Questions-------------------
  {
    path: "/questions",
    needsAdmin: true,
    needsQc: true,
    element: <Questions />,
  },
  {
    path: "/assigntomarker",
    needsAdmin: true,
    element: <AssignToMarker />,
  },
  {
    path: "/questions/edit/:id",
    needsAdmin: true,
    element: <QuestionEdit />,
  },
  {
    path: "/questions/create",
    needsAdmin: true,
    element: <QuestionCreate />,
  },

  // -----------------Candidate-------------------
  {
    path: "/candidate",
    needsAdmin: true,
    needsQc: true,
    
    element: <CandidateList />,
  },
  {
    path: "/candidate/create",
    // needsAdmin: true,
    // needsCand:true,
    element: <CandidateEdit />,
  },
  {
    path: "/candidate/:id",
    needsQc: true,
    needsAdmin: true,
    element: <CandidateEdit />,
  },
  {
    path: "/candidate/availableexams",
    needsCand: true,

    element: <AvailableExams />
  },
  // -----------------Candidate-------------------
  {
    path: '/candidate/Examination/:id',
    needsCand: true,
    element: <Examination />
  },
  {
    path: '/ExamsList',
    needsAdmin: true,
    needsQc: true,
    element: <ExamList />
  },
  //-----------------Marker-------------------
  {
    path: '/marker',
    // requireAuth: true,
    needsMarker:true,
    element: <MarkerList />
  },
  {
    path: '/marker/markexam',
    // requireAuth: true,
    needsMarker:true,
    element: <MarkExam />
  },
  //------------------------------------
  {
    path: '/ExamQuestionList/:id',
    needsAdmin: true,
    needsQc: true,
    element: <ExamQuestionList />
  },
  {
    path: '/AddQuestionToExam/:id',
    needsAdmin: true,
    needsQc: true,
    element: <AddQuestionToExam />
  },
  {
    path: '/candidate/ExamResults',
    element: <ExamResults />
  },
  {
    path: 'users',
    needsAdmin: true,
    element: <UsersList />
  },
  {
    path: 'users/edit/:email',
    needsAdmin: true,
    element: <UserForm />
  },
  {
    path: 'users/create',
    needsAdmin: true,
    element: <UserForm />
  },
  //this needs to stay as the last path
  {
    path: '/notauth',
    element: <NotAuth />
  },
  {
    path: '*',
    element: <NotFound />
  },

];

export default AppRoutes;
