import ReactDOM from 'react-dom';
import GoogleAuthButton from "./GoogleAuthButton.tsx";

const LoginModal = ({ onClose, onLoginSuccess }: { onClose: () => void; onLoginSuccess: () => void }) => {
    return ReactDOM.createPortal(
        <div className="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center">
            <div className="bg-white p-4 rounded-lg shadow-lg">
                <h2 className="text-lg font-semibold mb-2">Login Required</h2>
                <p className="mb-4">Please log in to perform a search.</p>
                <GoogleAuthButton
                    onLoginSuccess={onLoginSuccess}
                    onLoginRedirect={onClose}
                />
                <button
                    onClick={onClose}
                    className="mt-4 bg-gray-200 hover:bg-gray-300 text-gray-800 rounded-lg p-2"
                >
                    Close
                </button>
            </div>
        </div>,
        document.body
    );
};

export default LoginModal;
