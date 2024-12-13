import React, {useState} from "react";

interface PaginationProps {
    currentPage: number;
    pageCount: number;
    onPageChange: (page: number) => void;
}

const Pagination: React.FC<PaginationProps> = ({currentPage, pageCount, onPageChange}) => {
    const [isDebouncing, setIsDebouncing] = useState(false);

    const debounce = (callback: () => void, delay: number) => {
        if(isDebouncing) return;
        setIsDebouncing(true);
        callback();
        setTimeout(() => setIsDebouncing(false), delay);
    }

    const getPageNumbers = () => {
        const numbers = [];
        const totalPagesToShow = 5;
        const start = Math.max(0, currentPage - Math.floor(totalPagesToShow / 2));
        const end = Math.min(start + totalPagesToShow, pageCount)

        for (let i = start; i < end; i++) {
            numbers.push(i);
        }

        return numbers;
    }

    const handlePageClick = (page: number) => {
        if (page >= 0 && page < pageCount) {
            debounce(() => onPageChange(page), 300);
        }
    }

    return (
        <div className="flex items-center justify-center mt-6 space-x-2">
            <button
                onClick={() => handlePageClick(currentPage - 1)}
                disabled={currentPage === 0}
                className={`px-3 py-1 border rounded ${
                    currentPage === 0 ? "cursor-not-allowed opacity-50" : "hover:bg-gray-200"
                }`}
            >
                Previous
            </button>
            {getPageNumbers().map((page) => (
                <button
                    key={page}
                    onClick={() => handlePageClick(page)}
                    className={`px-3 py-1 border rounded ${
                        page === currentPage ? "bg-blue-500 text-white" : "hover:bg-gray-200"
                    }`}
                >
                    {page + 1}
                </button>
            ))}
            <button
                onClick={() => handlePageClick(currentPage + 1)}
                disabled={currentPage === pageCount - 1}
                className={`px-3 py-1 border rounded ${
                    currentPage === pageCount - 1
                        ? "cursor-not-allowed opacity-50"
                        : "hover:bg-gray-200"
                }`}
            >
                Next
            </button>
        </div>
    );
}

export default Pagination;