import {FaSearch} from 'react-icons/fa'
const SearchBar = () => {
    return (
        <form
              className="flex flex-col md:flex-row items-center p-4 bg-white rounded-lg shadow-md">
            <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 flex justify-between">
                <input
                    type="number"
                    placeholder="min price"
                    className="border border-gray-300 rounded-lg p-2 mb-2 md:mb-0 md:mr-2 w-full md:w-1/3"
                />
                <input
                    type="number"
                    placeholder="max price"
                    className="border border-gray-300 rounded-lg p-2 mb-2 md:mb-0 md:mr-2 w-full md:w-1/3"
                />
                <select
                    className="block appearance-none w-full bg-white border border-gray-300 rounded-lg p-2 mb-2 md:mb-0 md:mr-2 w-full md:w-1/4"
                >
                    <option key='1'>
                        Cadillac
                    </option>
                    <option key='2'>
                        Lincoln
                    </option>
                </select>
                <button type="submit" className="bg-blue-500 text-white rounded-lg p-2 mt-2 md:mt-0 md:ml-2">
                    <FaSearch className={'inline text-lg mb-1'}/> Szukaj
                </button>
            </div>
        </form>
    );
};

export default SearchBar;