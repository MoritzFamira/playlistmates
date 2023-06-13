// This is a React Router v6 app
import {
    BrowserRouter,
    Routes,
    Route,
    Link,
} from "react-router-dom";
import Signin from "./Signin.js"
import Signup from "./SignUp.js"
import Autocomplete from "./components/songSelect.js"
import EditTest from "./EditTest.js"
import Test from "./Test.js"

export default function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Signin />} />
                <Route path="/signup" element={<Signup />} />
                <Route path="/edittest" element={<EditTest />} />
                <Route path="/testing" element={<Test/>}/>
                <Route path="/auto" element={<Autocomplete />} />
            </Routes>
        </BrowserRouter>
    );
}

/*function Users() {
    return (
        <div>
            <nav>
                <Link to="me">My Profile</Link>
            </nav>

            <Routes>
                <Route path=":id" element={<Signup />} />
                <Route path="me" element={<Signin />} />
            </Routes>
        </div>
    );
}*/