import {Link} from "react-router-dom";

const MoreOffersButton = () => {
    return (
        <section className="m-auto max-w-lg my-10 px-6">
            <Link 
                to="/offers"
                className="block bg-black text-white text-center py-4 px-6 rounded-xl hover:bg-gray-700"
            >More Offers</Link>
        </section>
    );
};

export default MoreOffersButton;